using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMT.Agents.DataAssistant;
using SMT.ViewModel.Dto.AssistantDto;
using System.Threading;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    /// <summary>
    /// "Ask your data" - natural-language questions about plans, output, defects and FTQ,
    /// answered by an AI agent over read-only statistics queries. Requires login because
    /// every question is a paid model call.
    /// </summary>
    [Authorize]
    public class AssistantController : BaseController
    {
        private readonly IDataAssistantService _assistant;

        public AssistantController(IDataAssistantService assistant)
        {
            _assistant = assistant;
        }

        [HttpPost("Ask")]
        public async Task<ActionResult<AssistantAskResponse>> Ask([FromBody] AssistantAskRequest request, CancellationToken cancellationToken)
        {
            return Ok(await _assistant.AskAsync(request, cancellationToken));
        }
    }
}
