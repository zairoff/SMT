using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.MachineRepairerDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineRepairerController : ControllerBase
    {
        private readonly IMachineRepairerService _service;

        public MachineRepairerController(IMachineRepairerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReport([FromBody] MachineRepairerCreate machineRepairerCreate)
        {
            var result = await _service.AddAsync(machineRepairerCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
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
