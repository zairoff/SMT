using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.ViewModel.Dto.ProductBrandDto;
using SMT.Services.Interfaces;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    public class ProductBrandController : BaseController
    {
        private readonly IProductBrandService _service;

        public ProductBrandController(IProductBrandService service)
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
        [Route("GetByProductIdAsync")]
        public async Task<IActionResult> GetByProductIdAsync(int productId)
        {
            var result = await _service.GetByProductIdAsync(productId);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReport([FromBody] ProductBrandCreate productBrandCreate)
        {
            var result = await _service.AddAsync(productBrandCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReport(int id, [FromBody] ProductBrandUpdate productBrandUpdate)
        {
            var result = await _service.UpdateAsync(id, productBrandUpdate);

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
