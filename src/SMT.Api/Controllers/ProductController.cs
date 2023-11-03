using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.ViewModel.Dto.ProductDto;
using SMT.Services.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace SMT.Api.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] bool? isActive)
        {
            var result = await _service.GetAllAsync(isActive ?? true);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetAsync(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _service.GetByNameAsync(name);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReport([FromBody] ProductCreate productCreate)
        {
            var result = await _service.AddAsync(productCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReport(int id, [FromBody] ProductUpdate productUpdate)
        {
            var result = await _service.UpdateAsync(id, productUpdate);

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
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
