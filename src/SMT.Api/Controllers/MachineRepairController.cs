using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.MachineRepairDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineRepairController : ControllerBase
    {
        private readonly IMachineRepairService _service;

        public MachineRepairController(IMachineRepairService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string shift)
        {
            var result = await _service.GetAllAsync(shift);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetAsync(id);

            return Ok(result);
        }

        [HttpGet("GetByMachine")]
        public async Task<IActionResult> GetByMachine(int machineId, string shift)
        {
            var result = await _service.GetByMachineIdAsync(machineId, shift);

            return Ok(result);
        }

        [HttpGet("GetByMonth")]
        public async Task<IActionResult> GetByMonth(string shift, string month)
        {
            var result = await _service.GetByMonthAsync(shift, month);

            return Ok(result);
        }

        [HttpGet("ByMachineIdAndDate")]
        public async Task<IActionResult> GetByMachineIdAndDate(int machineId, string shift, string date)
        {
            var result = await _service.GetByMachineIdAndDateAsync(machineId, shift, date);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReport([FromBody] MachineRepairCreate machineRepairCreate)
        {
            var result = await _service.AddAsync(machineRepairCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] MachineRepairUpdate machineRepairUpdate)
        {
            var result = await _service.UpdateAsync(id, machineRepairUpdate);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var result = await _service.DeleteAsync(id);

            return Ok(result);
        }
    }
}
