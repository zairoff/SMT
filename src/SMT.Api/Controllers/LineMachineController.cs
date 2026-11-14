using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces.Feeder;
using SMT.ViewModel.Dto.LineMachineDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Authorize]
    public class LineMachineController : BaseController
    {
        private readonly ILineMachineService _service;

        public LineMachineController(ILineMachineService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("line/{lineId}")]
        public async Task<IActionResult> GetByLine(int lineId)
        {
            var result = await _service.GetByLineAsync(lineId);

            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = FeederRoles.SetupManagers)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Save([FromBody] LineMachineSave save)
        {
            var result = await _service.SaveAsync(save);

            return Ok(result);
        }

        [HttpPost("line/{lineId}/module")]
        [Authorize(Roles = FeederRoles.SetupManagers)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> AddModule(int lineId, [FromBody] MachineModuleSave save)
        {
            var result = await _service.AddModuleAsync(lineId, save);

            return Ok(result);
        }

        [HttpPut("module/{moduleId}")]
        [Authorize(Roles = FeederRoles.SetupManagers)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateModule(int moduleId, [FromBody] MachineModuleSave save)
        {
            var result = await _service.UpdateModuleAsync(moduleId, save);

            return Ok(result);
        }

        [HttpPost("module/{moduleId}/slots")]
        [Authorize(Roles = FeederRoles.SetupManagers)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddSlots(int moduleId, [FromBody] FeederSlotsCreate create)
        {
            var result = await _service.AddSlotsAsync(moduleId, create);

            return Ok(result);
        }

        [HttpPut("slot/{slotId}/active")]
        [Authorize(Roles = FeederRoles.SetupManagers)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetSlotActive(int slotId, bool isActive)
        {
            var result = await _service.SetSlotActiveAsync(slotId, isActive);

            return Ok(result);
        }
    }
}
