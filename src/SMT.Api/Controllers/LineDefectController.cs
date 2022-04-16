using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.LineDefectDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineDefectController : BaseController
    {
        private readonly ILineDefectService _service;

        public LineDefectController(ILineDefectService service)
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

        [HttpGet]
        [Route("GetByLineId")]
        public async Task<IActionResult> GetByProductIdAsync(int lineId)
        {
            var result = await _service.GetByLineIdAsync(lineId);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByLineAndDefectId")]
        public async Task<IActionResult> GetByLineAndDefectIdAsync(int lineId, int defectId)
        {
            var result = await _service.GetByLineAndDefectIdAsync(lineId, defectId);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReport([FromBody] LineDefectCreate lineDefectCreate)
        {
            var result = await _service.AddAsync(lineDefectCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReport(int id, [FromBody] LineDefectUpdate lineDefectUpdate)
        {
            var result = await _service.UpdateAsync(id, lineDefectUpdate);

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
