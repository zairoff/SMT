using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ProductTransactionDto;
using System.Threading.Tasks;
using System;
using SMT.Services.Interfaces.ReturnedProducts;
using SMT.Domain.ReturnedProducts;
using SMT.ViewModel.Dto.ReturnedProductTransactionDto;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnedProductTransactionController : ControllerBase
    {
        private readonly IReturnedProductTransactionService _service;

        public ReturnedProductTransactionController(IReturnedProductTransactionService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetByDate")]
        public async Task<IActionResult> GetByDate([FromQuery] DateTime dateTime, [FromQuery] ReturnedProductTransactionType transactionType)
        {
            var result = await _service.GetByDateAsync(dateTime, transactionType);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByDateGroupBy")]
        public async Task<IActionResult> GetByDateGroupBy([FromQuery] DateTime dateTime, [FromQuery] ReturnedProductTransactionType transactionType)
        {
            var result = await _service.GetByDateGroupByAsync(dateTime, transactionType);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByDateRange")]
        public async Task<IActionResult> GetByDateRange(DateTime from, DateTime to, ReturnedProductTransactionType transactionType)
        {
            var result = await _service.GetByDateRangeAsync(from, to, transactionType);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByDateRangeGroupBy")]
        public async Task<IActionResult> GetByDateRangeGroupBy(DateTime from, DateTime to, ReturnedProductTransactionType transactionType)
        {
            var result = await _service.GetByDateRangeGroupByAsync(from, to, transactionType);

            return Ok(result);
        }

        [HttpPost("ImportFromFactory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportFromFactory([FromBody] ReturnedProductTransactionCreate returnedProductTransactionCreate)
        {
            await _service.ImportFromFactoryAsync(returnedProductTransactionCreate);

            return new OkResult();
        }

        [HttpPost("ImportFromRepair")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportFromRepair([FromBody] ReturnedProductTransactionCreate returnedProductTransactionCreate)
        {
            await _service.ImportFromRepairAsync(returnedProductTransactionCreate);

            return new OkResult();
        }

        [HttpPost("ImportFromRepairToUtilize")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ImportFromRepairToUtilize([FromBody] ReturnedProductTransactionCreate returnedProductTransactionCreate)
        {
            await _service.ImportFromRepairToUtilizeAsync(returnedProductTransactionCreate);

            return new OkResult();
        }

        [HttpPost("ExportToFactory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExportToFactory([FromBody] ReturnedProductTransactionCreate returnedProductTransactionCreate)
        {
            await _service.ExportToFactoryAsync(returnedProductTransactionCreate);

            return new OkResult();
        }

        [HttpPost("ExportToRepair")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExportToRepair([FromBody] ReturnedProductTransactionCreate returnedProductTransactionCreate)
        {
            await _service.ExportToRepairAsync(returnedProductTransactionCreate);

            return new OkResult();
        }

        [HttpPost("ExportToUtilize")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExportToUtilize([FromBody] ReturnedProductTransactionCreate returnedProductTransactionCreate)
        {
            await _service.ExportToUtilizeAsync(returnedProductTransactionCreate);

            return new OkResult();
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
