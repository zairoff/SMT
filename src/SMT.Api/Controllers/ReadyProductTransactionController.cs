using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ProductTransactionDto;
using System.Threading.Tasks;
using System;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadyProductTransactionController : BaseController
    {
        private readonly IReadyProductTransactionService _service;

        public ReadyProductTransactionController(IReadyProductTransactionService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByProductBrand")]
        public async Task<IActionResult> GetProductBrand([FromQuery] int productBrandId)
        {
            var result = await _service.GetByProductBrandAsync(productBrandId);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByProduct")]
        public async Task<IActionResult> GetByProduct([FromQuery] int productId)
        {
            var result = await _service.GetByProductAsync(productId);

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
        [Route("GetByDateGroupBy")]
        public async Task<IActionResult> GetByDateGroupBy([FromQuery] DateTime dateTime, [FromQuery] TransactionType transactionType)
        {
            var result = await _service.GetByDateGroupByAsync(dateTime, transactionType);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByDateRange")]
        public async Task<IActionResult> GetByDateRange(DateTime from, DateTime to, TransactionType transactionType)
        {
            var result = await _service.GetByDateRangeAsync(from, to, transactionType);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByDateRangeGroupBy")]
        public async Task<IActionResult> GetByDateRangeGroupBy(DateTime from, DateTime to, TransactionType transactionType)
        {
            var result = await _service.GetByDateRangeGroupByAsync(from, to, transactionType);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetBySapCodeDateRange")]
        public async Task<IActionResult> GetBySapCodeDateRange(string sapCode, DateTime from, DateTime to, TransactionType transactionType)
        {
            var result = await _service.GetBySapCodeDateRange(sapCode, from, to, transactionType);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Import([FromBody] ReadyProductTransactionImport readyProductTransactionImport)
        {
            await _service.ImportAsync(readyProductTransactionImport);

            return new OkResult();
        }

        [HttpPost]
        [Route("/Notify/GroupBy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GroupByNotify()
        {
            await _service.GroupByNotifyAsync();

            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Export([FromBody] ReadyProductTransactionExport readyProductTransactionExport)
        {
            await _service.ExportAsync(readyProductTransactionExport);

            return Ok();
        }

        [HttpPut]
        [Route("{id}/change")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Change([FromRoute] int id, [FromQuery] int count)
        {
            await _service.ChangesAsync(id, count);

            return Ok();
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
