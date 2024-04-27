using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ComponentDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentController : ControllerBase
    {
        private readonly IComponentService _service;

        public ComponentController(IComponentService service)
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
        [Route("GetByPartNumber")]
        public async Task<IActionResult> GetByPartNumber(string partNumber)
        {
            var result = await _service.GetByPartNumberAsync(partNumber);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByRcode")]
        public async Task<IActionResult> GetByRcode(string rcode)
        {
            var result = await _service.GetByRcodeAsync(rcode);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByStorePlace")]
        public async Task<IActionResult> GetByStorePlace(string storePlace)
        {
            var result = await _service.GetByStorePlaceAsync(storePlace);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateComponent([FromBody] ComponentCreate componentCreate)
        {
            var result = await _service.AddAsync(componentCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateComponent(int id, [FromBody] ComponentUpdate componentUpdate)
        {
            var result = await _service.UpdateAsync(id, componentUpdate);

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
