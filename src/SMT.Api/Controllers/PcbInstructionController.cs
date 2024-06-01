using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.PcbInstructionDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PcbInstructionController : ControllerBase
    {
        private readonly IPcbInstructionService _service;

        public PcbInstructionController(IPcbInstructionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool? isActive)
        {
            var result = await _service.GetAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetAsync(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByPosition")]
        public async Task<IActionResult> GetByPosition(int position)
        {
            var result = await _service.GetByPositionAsync(position);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateInstruction([FromBody] PcbInstructionCreate pcbInstructionCreate)
        {
            await _service.AddAsync(pcbInstructionCreate);

            return Ok();
        }
    }
}
