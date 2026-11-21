using Anthropic.Models.Beta.Messages;
using SMT.Access.Repository.Statics;
using SMT.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SMT.Agents.DataAssistant
{
    /// <summary>
    /// The read-only tools the data assistant may call. Each one wraps an existing
    /// repository/service query - the model never sees SQL or the DbContext, and
    /// nothing here writes data.
    /// </summary>
    public class DataAssistantTools
    {
        // Wide enough for "this year vs last year" questions, narrow enough that one
        // question can't make the statics queries scan the whole history.
        private const int MaxRangeDays = 400;

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        private readonly IStaticsRepository _statics;
        private readonly ILineService _lineService;

        public DataAssistantTools(IStaticsRepository statics, ILineService lineService)
        {
            _statics = statics;
            _lineService = lineService;
        }

        public static IReadOnlyList<BetaToolUnion> Definitions { get; } = new List<BetaToolUnion>
        {
            Tool("list_lines",
                "List the production lines with their numeric ids and names. Call this first whenever the " +
                "user names a line (e.g. \"line 3\", \"SMT-2\") so you can pass the right line_id to other tools.",
                new Dictionary<string, object>(),
                Array.Empty<string>()),

            Tool("defect_ranking",
                "Count defect reports in a date range, grouped by model, product, brand, line or defect type, " +
                "sorted from most to least. Each defect report is one defect found on a board. " +
                "Use for questions like \"top defects\", \"which model has the most defects\", \"defects per line\". " +
                "line_id is only applied when group_by is \"defect\" (defect types on one line).",
                new Dictionary<string, object>
                {
                    ["group_by"] = new { type = "string", @enum = new[] { "model", "product", "brand", "line", "defect" } },
                    ["from"] = new { type = "string", description = "Start date, yyyy-MM-dd, inclusive" },
                    ["to"] = new { type = "string", description = "End date, yyyy-MM-dd, inclusive" },
                    ["line_id"] = new { type = "integer", description = "Optional; only with group_by = defect" },
                },
                new[] { "group_by", "from", "to" }),

            Tool("daily_plan",
                "Planned vs produced board counts per day in a date range, optionally for one line. " +
                "Days without a plan are returned with zeros.",
                new Dictionary<string, object>
                {
                    ["from"] = new { type = "string", description = "Start date, yyyy-MM-dd, inclusive" },
                    ["to"] = new { type = "string", description = "End date, yyyy-MM-dd, inclusive" },
                    ["line_id"] = new { type = "integer", description = "Optional line id; omit for all lines" },
                },
                new[] { "from", "to" }),

            Tool("daily_quality",
                "FTQ (first time quality, %) and produced count per day in a date range, optionally for one line. " +
                "FTQ = 100 - defects * 100 / produced. A day with produced = 0 has ftq 0 meaning \"no production\", not bad quality.",
                new Dictionary<string, object>
                {
                    ["from"] = new { type = "string", description = "Start date, yyyy-MM-dd, inclusive" },
                    ["to"] = new { type = "string", description = "End date, yyyy-MM-dd, inclusive" },
                    ["line_id"] = new { type = "integer", description = "Optional line id; omit for all lines" },
                },
                new[] { "from", "to" }),

            Tool("day_breakdown",
                "For one day: every line x model with planned, produced, defect count and FTQ %. " +
                "Use to explain a specific day (\"why was FTQ low on Monday\", \"what did line 2 build yesterday\").",
                new Dictionary<string, object>
                {
                    ["date"] = new { type = "string", description = "The day, yyyy-MM-dd" },
                    ["line_id"] = new { type = "integer", description = "Optional line id; omit for all lines" },
                },
                new[] { "date" }),
        };

        /// <summary>
        /// Runs one tool call. Returns the JSON result, or throws <see cref="ToolInputException"/>
        /// for bad input - the caller sends that back to the model as an error result so it can retry.
        /// </summary>
        public async Task<string> ExecuteAsync(string name, IReadOnlyDictionary<string, JsonElement> input)
        {
            switch (name)
            {
                case "list_lines":
                    var lines = await _lineService.GetAllAsync(null);
                    return Serialize(lines.Select(l => new { l.Id, l.Name, l.IsActive }));

                case "defect_ranking":
                {
                    var (from, to) = DateRange(input);
                    var groupBy = String(input, "group_by");
                    var lineId = OptionalInt(input, "line_id");
                    switch (groupBy)
                    {
                        case "model": return Serialize(await _statics.GroupByModelAsync(from, to));
                        case "product": return Serialize(await _statics.GroupByProductAsync(from, to));
                        case "brand": return Serialize(await _statics.GroupByBrandAsync(from, to));
                        case "line": return Serialize(await _statics.GroupByLineAsync(from, to));
                        case "defect":
                            return Serialize(lineId.HasValue
                                ? await _statics.GroupByDefectAsync(lineId.Value, from, to)
                                : await _statics.GroupByDefectAsync(from, to));
                        default:
                            throw new ToolInputException("group_by must be one of model, product, brand, line, defect");
                    }
                }

                case "daily_plan":
                {
                    var (from, to) = DateRange(input);
                    return Serialize(await _statics.GetPlanStaticsAsync(from, to, OptionalInt(input, "line_id")));
                }

                case "daily_quality":
                {
                    var (from, to) = DateRange(input);
                    return Serialize(await _statics.GetQualityStaticsAsync(from, to, OptionalInt(input, "line_id")));
                }

                case "day_breakdown":
                {
                    var date = Date(input, "date");
                    var lineId = OptionalInt(input, "line_id");
                    var plan = (await _statics.GetPlanDetailsByDateAsync(date, lineId)).ToList();
                    var quality = (await _statics.GetQualityDetailsByDateAsync(date, lineId)).ToList();
                    return Serialize(plan.Select(p =>
                    {
                        var q = quality.FirstOrDefault(x => x.LineId == p.LineId && x.ModelId == p.ModelId);
                        return new
                        {
                            p.LineId,
                            p.LineName,
                            p.ModelName,
                            p.Planned,
                            p.Produced,
                            DefectCount = q?.DefectCount ?? 0,
                            Ftq = q?.Ftq ?? 0,
                        };
                    }));
                }

                default:
                    throw new ToolInputException($"Unknown tool '{name}'");
            }
        }

        /// <summary>Short human-readable form of a call, shown under the answer as its source.</summary>
        public static string Describe(string name, IReadOnlyDictionary<string, JsonElement> input)
        {
            var args = string.Join(", ", input.Select(kv => $"{kv.Key}={kv.Value}"));
            return $"{name}({args})";
        }

        private static BetaToolUnion Tool(string name, string description, Dictionary<string, object> properties, string[] required)
        {
            return new BetaTool
            {
                Name = name,
                Description = description,
                InputSchema = new InputSchema
                {
                    Properties = properties.ToDictionary(p => p.Key, p => JsonSerializer.SerializeToElement(p.Value)),
                    Required = required,
                },
            };
        }

        private static string Serialize(object value) => JsonSerializer.Serialize(value, JsonOptions);

        private static (DateTime from, DateTime to) DateRange(IReadOnlyDictionary<string, JsonElement> input)
        {
            var from = Date(input, "from");
            var to = Date(input, "to");
            if (to < from)
                throw new ToolInputException("'to' is before 'from'");
            if ((to - from).TotalDays > MaxRangeDays)
                throw new ToolInputException($"Date range is limited to {MaxRangeDays} days; split the question into smaller ranges");
            return (from, to);
        }

        private static DateTime Date(IReadOnlyDictionary<string, JsonElement> input, string key)
        {
            var text = String(input, key);
            if (!DateTime.TryParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                throw new ToolInputException($"'{key}' must be a date in yyyy-MM-dd format, got '{text}'");
            return date;
        }

        private static string String(IReadOnlyDictionary<string, JsonElement> input, string key)
        {
            if (!input.TryGetValue(key, out var value) || value.ValueKind != JsonValueKind.String)
                throw new ToolInputException($"'{key}' is required and must be a string");
            return value.GetString();
        }

        private static int? OptionalInt(IReadOnlyDictionary<string, JsonElement> input, string key)
        {
            if (!input.TryGetValue(key, out var value) || value.ValueKind == JsonValueKind.Null)
                return null;
            if (value.ValueKind == JsonValueKind.Number && value.TryGetInt32(out var number))
                return number;
            throw new ToolInputException($"'{key}' must be an integer");
        }
    }

    public class ToolInputException : Exception
    {
        public ToolInputException(string message) : base(message)
        {
        }
    }
}
