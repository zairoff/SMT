using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Domain.Feeder;
using SMT.Services.Interfaces.Feeder;
using SMT.ViewModel.Dto.FeederSetupDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    // Open to everyone on the line - no login. Whoever is logged in (if anyone) is recorded.
    public class FeederSetupController : BaseController
    {
        private readonly IFeederSetupService _service;

        public FeederSetupController(IFeederSetupService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int? lineId, int? modelId, FeederSetupStatus? status)
        {
            var result = await _service.GetAllAsync(lineId, modelId, status);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetAsync(id);

            return Ok(result);
        }

        [HttpGet("{id}/diff")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Diff(int id, int? againstId)
        {
            var result = await _service.DiffAsync(id, againstId);

            return Ok(result);
        }

        [HttpPost("preview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Preview([FromBody] FeederSetupImport import)
        {
            var result = await _service.PreviewAsync(import);

            return Ok(result);
        }

        [HttpPost("import")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Import([FromBody] FeederSetupImport import)
        {
            var result = await _service.ImportAsync(import, FeederUser.Username(User));

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}/activate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Activate(int id)
        {
            var result = await _service.ActivateAsync(id, FeederUser.Username(User));

            return Ok(result);
        }

        [HttpPost("{id}/copy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CopyToLine(int id, [FromBody] FeederSetupCopy copy)
        {
            var result = await _service.CopyToLineAsync(id, copy, FeederUser.Username(User));

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            return Ok(result);
        }
    }
}
