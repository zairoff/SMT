using Anthropic;
using Anthropic.Exceptions;
using Anthropic.Models.Beta.Messages;
using Microsoft.Extensions.Logging;
using SMT.Services.Exceptions;
using SMT.ViewModel.Dto.AssistantDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SMT.Agents.DataAssistant
{
    public interface IDataAssistantService
    {
        Task<AssistantAskResponse> AskAsync(AssistantAskRequest request, CancellationToken cancellationToken);
    }

    public class DataAssistantOptions
    {
        public string ApiKey { get; set; }

        public string Model { get; set; } = "claude-opus-5-5";

        /// <summary>Upper bound on model round-trips per question; each round may run several tools.</summary>
        public int MaxRounds { get; set; } = 8;
    }

    /// <summary>
    /// "Ask your data": answers a free-text question about production/quality by letting Claude
    /// call the read-only tools in <see cref="DataAssistantTools"/>, then returning its answer
    /// plus an optional table.
    /// </summary>
    public class DataAssistantService : IDataAssistantService
    {
        private const int MaxQuestionLength = 2000;
        private const int MaxHistoryTurns = 10;

        private const string SystemPrompt =
@"You are the data assistant of an SMT / PCB assembly factory's production control system.
Supervisors and managers ask you questions about production plans, output, defects and FTQ (first time quality).

Answer only from data returned by your tools. Never invent or estimate numbers. If the tools cannot answer
the question (for example it needs data you have no tool for, such as feeder scans, machine repairs or
component stock), say so plainly and say which questions you can answer instead.

Working rules:
- Resolve line names to ids with list_lines before passing line_id.
- Interpret relative dates (""yesterday"", ""last week"", ""this month"") from the date given in the question.
- Call several tools in parallel when they are independent.
- When comparing periods or lines, compute the differences yourself from the tool results.

Answer format:
- ""answer"": a short, direct answer (1-4 sentences) with the key figures, in the requested language.
  Plain text, no markdown.
- ""columns"" / ""rows"": a table of the figures behind the answer when there is more than one
  row of data worth showing (all cells as strings, at most 50 rows); otherwise both empty.
  Column headers in the requested language.";

        // Final answer shape. Structured outputs guarantee the last message parses.
        private static readonly Dictionary<string, JsonElement> AnswerSchema = new Dictionary<string, JsonElement>
        {
            ["type"] = JsonSerializer.SerializeToElement("object"),
            ["properties"] = JsonSerializer.SerializeToElement(new
            {
                answer = new { type = "string" },
                columns = new { type = "array", items = new { type = "string" } },
                rows = new { type = "array", items = new { type = "array", items = new { type = "string" } } },
            }),
            ["required"] = JsonSerializer.SerializeToElement(new[] { "answer", "columns", "rows" }),
            ["additionalProperties"] = JsonSerializer.SerializeToElement(false),
        };

        private static readonly Dictionary<string, string> LanguageNames = new Dictionary<string, string>
        {
            ["en"] = "English",
            ["ru"] = "Russian",
            ["uz"] = "Uzbek (Latin script)",
        };

        private readonly AnthropicClient _client;
        private readonly DataAssistantOptions _options;
        private readonly DataAssistantTools _tools;
        private readonly ILogger<DataAssistantService> _logger;

        public DataAssistantService(AnthropicClient client, DataAssistantOptions options, DataAssistantTools tools,
            ILogger<DataAssistantService> logger)
        {
            _client = client;
            _options = options;
            _tools = tools;
            _logger = logger;
        }

        public async Task<AssistantAskResponse> AskAsync(AssistantAskRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_options.ApiKey))
                throw new ServiceUnavailableException("The data assistant is not configured (missing Anthropic API key).");

            var question = request?.Question?.Trim();
            if (string.IsNullOrEmpty(question))
                throw new InvalidOperationException("Question is empty.");
            if (question.Length > MaxQuestionLength)
                throw new InvalidOperationException($"Question is longer than {MaxQuestionLength} characters.");

            var language = LanguageNames.TryGetValue(request.Language ?? "", out var name) ? name : "English";
            var messages = BuildHistory(request.History);
            messages.Add(new BetaUserMessageParam
            {
                // Date and language go here, not in the system prompt, so the system prompt stays cacheable.
                Content = $"Today is {DateTime.Today:yyyy-MM-dd} ({DateTime.Today:dddd}). Answer in {language}.\n\nQuestion: {question}",
            });

            var sources = new List<string>();

            for (var round = 0; round < _options.MaxRounds; round++)
            {
                var response = await CreateAsync(messages, cancellationToken);

                _logger.LogInformation("Data assistant round {Round}: stop={StopReason} model={Model} in={InputTokens} out={OutputTokens}",
                    round, response.StopReason, response.Model, response.Usage.InputTokens, response.Usage.OutputTokens);

                if (response.StopReason == "refusal")
                    return new AssistantAskResponse { Answer = "Sorry, I can't help with that question.", Sources = sources };

                if (response.StopReason == "max_tokens")
                    break;

                var toolUses = response.Content
                    .Select(b => b.TryPickToolUse(out var t) ? t : null)
                    .Where(t => t != null)
                    .ToList();

                if (response.StopReason != "tool_use" || toolUses.Count == 0)
                    return ParseFinalAnswer(response, sources);

                messages.Add(new BetaAssistantMessageParam { Content = EchoContent(response.Content) });

                // All tool results go back in ONE user message (keeps parallel tool use working).
                // Tools share one DbContext, so they run one after another, not concurrently.
                var results = new List<BetaContentBlockParam>();
                foreach (var toolUse in toolUses)
                {
                    sources.Add(DataAssistantTools.Describe(toolUse.Name, toolUse.Input));
                    results.Add(await RunToolAsync(toolUse));
                }
                messages.Add(new BetaUserMessageParam { Content = results });
            }

            _logger.LogWarning("Data assistant gave up after {Rounds} rounds for question: {Question}", _options.MaxRounds, question);
            return new AssistantAskResponse
            {
                Answer = "I could not finish answering this question. Try asking something narrower (one line, a shorter period).",
                Sources = sources,
            };
        }

        private async Task<BetaMessage> CreateAsync(List<BetaMessageParam> messages, CancellationToken cancellationToken)
        {
            try
            {
                return await _client.Beta.Messages.Create(new MessageCreateParams
                {
                    Model = _options.Model,
                    MaxTokens = 16000,
                    System = SystemPrompt,
                    Tools = DataAssistantTools.Definitions.ToList(),
                    Messages = messages.ToList(),
                    OutputConfig = new BetaOutputConfig
                    {
                        // Data Q&A over a handful of tools: medium is plenty and keeps answers quick.
                        Effort = Effort.Medium,
                        Format = new BetaJsonOutputFormat { Schema = AnswerSchema },
                    },
                    // If the model declines on a safety classifier, the server retries on its default fallback model.
                    Betas = ["server-side-fallback-2026-07-01"],
                    Fallbacks = new Default(),
                }, cancellationToken);
            }
            catch (AnthropicRateLimitException ex)
            {
                _logger.LogWarning(ex, "Anthropic rate limit hit");
                throw new ServiceUnavailableException("The data assistant is busy right now. Please try again in a minute.");
            }
            catch (AnthropicApiException ex)
            {
                _logger.LogError(ex, "Anthropic API call failed");
                throw new ServiceUnavailableException("The data assistant is temporarily unavailable.");
            }
        }

        private async Task<BetaContentBlockParam> RunToolAsync(BetaToolUseBlock toolUse)
        {
            try
            {
                var json = await _tools.ExecuteAsync(toolUse.Name, toolUse.Input);
                return new BetaToolResultBlockParam { ToolUseID = toolUse.ID, Content = json };
            }
            catch (ToolInputException ex)
            {
                // Bad arguments: tell the model so it can correct the call.
                return new BetaToolResultBlockParam { ToolUseID = toolUse.ID, Content = ex.Message, IsError = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Data assistant tool {Tool} failed", toolUse.Name);
                return new BetaToolResultBlockParam { ToolUseID = toolUse.ID, Content = "The query failed on the server.", IsError = true };
            }
        }

        private static List<BetaMessageParam> BuildHistory(List<AssistantTurn> history)
        {
            // Earlier turns are replayed as plain text Q&A. The API needs user/assistant to
            // alternate starting with user, so keep only well-formed user->assistant pairs.
            var turns = (history ?? new List<AssistantTurn>())
                .Where(t => !string.IsNullOrWhiteSpace(t?.Text))
                .ToList();

            var pairs = new List<(string question, string answer)>();
            for (var i = 0; i + 1 < turns.Count; i++)
            {
                if (turns[i].Role == "user" && turns[i + 1].Role == "assistant")
                {
                    pairs.Add((turns[i].Text, turns[i + 1].Text));
                    i++;
                }
            }

            var messages = new List<BetaMessageParam>();
            foreach (var (q, a) in pairs.Skip(Math.Max(0, pairs.Count - MaxHistoryTurns / 2)))
            {
                messages.Add(new BetaUserMessageParam { Content = Truncate(q, MaxQuestionLength) });
                messages.Add(new BetaAssistantMessageParam { Content = Truncate(a, 4000) });
            }
            return messages;
        }

        /// <summary>
        /// Rebuilds the assistant turn for the follow-up request (the SDK has no ToParam()).
        /// Thinking blocks are passed back unchanged. If the server switched models mid-output
        /// (a "fallback" block), the declined model's thinking/tool_use blocks before the last
        /// fallback block are dropped and the fallback block stays in place, as the API requires.
        /// </summary>
        private static List<BetaContentBlockParam> EchoContent(IReadOnlyList<BetaContentBlock> content)
        {
            var lastFallback = -1;
            for (var i = 0; i < content.Count; i++)
                if (content[i].TryPickFallback(out _))
                    lastFallback = i;

            var echoed = new List<BetaContentBlockParam>();
            for (var i = 0; i < content.Count; i++)
            {
                var block = content[i];
                var beforeBoundary = i < lastFallback;

                if (block.TryPickText(out var text))
                    echoed.Add(new BetaTextBlockParam { Text = text.Text });
                else if (block.TryPickFallback(out var fallback))
                    echoed.Add(new BetaFallbackBlockParam
                    {
                        From = new BetaFallbackInfoParam { Model = fallback.From.Model },
                        To = new BetaFallbackInfoParam { Model = fallback.To.Model },
                    });
                else if (beforeBoundary)
                    continue;
                else if (block.TryPickThinking(out var thinking))
                    echoed.Add(new BetaThinkingBlockParam { Thinking = thinking.Thinking, Signature = thinking.Signature });
                else if (block.TryPickRedactedThinking(out var redacted))
                    echoed.Add(new BetaRedactedThinkingBlockParam { Data = redacted.Data });
                else if (block.TryPickToolUse(out var toolUse))
                    echoed.Add(new BetaToolUseBlockParam { ID = toolUse.ID, Name = toolUse.Name, Input = toolUse.Input });
            }
            return echoed;
        }

        private AssistantAskResponse ParseFinalAnswer(BetaMessage response, List<string> sources)
        {
            var text = string.Concat(response.Content
                .Select(b => b.TryPickText(out var t) ? t.Text : null)
                .Where(t => t != null));

            try
            {
                using var doc = JsonDocument.Parse(text);
                var root = doc.RootElement;
                var columns = root.GetProperty("columns").EnumerateArray().Select(c => c.GetString()).ToList();
                var rows = root.GetProperty("rows").EnumerateArray()
                    .Select(r => r.EnumerateArray().Select(c => c.GetString()).ToList())
                    .Take(50)
                    .ToList();

                return new AssistantAskResponse
                {
                    Answer = root.GetProperty("answer").GetString(),
                    Columns = columns,
                    Rows = rows,
                    Sources = sources,
                };
            }
            catch (Exception ex) when (ex is JsonException || ex is KeyNotFoundException || ex is InvalidOperationException)
            {
                // Should not happen with structured outputs; fall back to showing the raw text.
                _logger.LogWarning(ex, "Data assistant returned unparseable answer");
                return new AssistantAskResponse { Answer = text, Sources = sources };
            }
        }

        private static string Truncate(string value, int max) => value.Length <= max ? value : value.Substring(0, max);
    }
}
