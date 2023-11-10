using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ProductTransactionDto;
using SMT.ViewModel.Dto.ReadyProductDto;
using System;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadyProductController : BaseController
    {
        private readonly IReadyProductService _service;

        public ReadyProductController(IReadyProductService service)
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
        [Route("GetByDate")]
        public async Task<IActionResult> GetByDate([FromQuery] DateTime dateTime, [FromQuery] TransactionType transactionType)
        {
            var result = await _service.GetByDateAsync(dateTime, transactionType);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByDateRange")]
        public async Task<IActionResult> GetByDateRange(DateTime from, DateTime to, TransactionType transactionType)
        {
            var result = await _service.GetByDateRangeAsync(from, to, transactionType);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Import([FromBody] ReadyProductCreate readyProductCreate)
        {
            var result = await _service.ImportAsync(readyProductCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Export([FromBody] ReadyProductUpdate readyProductUpdate)
        {
            var result = await _service.ExportAsync(readyProductUpdate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteTransactionAsync(id);

            return Ok(result);
        }
    }
}
