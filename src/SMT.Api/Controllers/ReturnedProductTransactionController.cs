using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using SMT.Services.Interfaces.ReturnedProducts;
using SMT.Domain.ReturnedProducts;
using SMT.ViewModel.Dto.ReturnedProductTransactionDto;
using System.Collections;
using System.Collections.Generic;

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
        [Route("GetStoreState")]
        public async Task<IActionResult> GetStoreState()
        {
            var result = await _service.GetStoreStateAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("GetRepairState")]
        public async Task<IActionResult> GetRepairState()
        {
            var result = await _service.GetRepairStateAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("GetUtilizeState")]
        public async Task<IActionResult> GetUtilizeState()
        {
            var result = await _service.GetUtilizeStateAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("GetBufferState")]
        public async Task<IActionResult> GetBufferState()
        {
            var result = await _service.GetBufferStateAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("GetState")]
        public async Task<IActionResult> GetState(ReturnedProductTransactionType transactionType)
        {
            IEnumerable<ReturnedProductTransactionResponse> result = null;

            switch (transactionType)
            {
                case ReturnedProductTransactionType.ExportFromBufferToRepair:
                    result = await _service.GetBufferStateAsync();
                    break;
                case ReturnedProductTransactionType.ExportFromRepairToStore:
                case ReturnedProductTransactionType.ExportFromRepairToUtilize:
                    result = await _service.GetRepairStateAsync();
                    break;
                case ReturnedProductTransactionType.ExportFromStoreToUtilize:
                    result = await _service.GetUtilizeStateAsync();
                    break;
                case ReturnedProductTransactionType.ExportFromStoreToFactory:
                    result = await _service.GetStoreStateAsync();
                    break;

                default:
                    break;
            }

            return Ok(result);
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

        [HttpPost]
        [Route("Notify/GroupBy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GroupByNotify()
        {
            await _service.GroupByNotifyAsync();

            return Ok();
        }

        [HttpPost("import")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Import([FromBody] ReturnedProductTransactionCreate returnedProductTransactionCreate)
        {
            switch (returnedProductTransactionCreate.TransactionType)
            {
                case ReturnedProductTransactionType.ImportFromFactoryToBuffer:
                    await _service.ImportFromFactoryToBufferAsync(returnedProductTransactionCreate);
                    break;
                default:
                    break;
            }

            return new OkResult();
        }

        [HttpPost("export")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Export([FromBody] ReturnedProductTransactionCreate returnedProductTransactionCreate)
        {
            switch (returnedProductTransactionCreate.TransactionType)
            {
                case ReturnedProductTransactionType.ExportFromStoreToFactory:
                    await _service.ExportFromStoreToFactoryAsync(returnedProductTransactionCreate);
                    break;
                case ReturnedProductTransactionType.ExportFromBufferToRepair:
                    await _service.ExportFromBufferToRepairAsync(returnedProductTransactionCreate);
                    break;
                case ReturnedProductTransactionType.ExportFromStoreToUtilize:
                    await _service.ExportFromStoreToUtilizeAsync(returnedProductTransactionCreate);
                    break;
                case ReturnedProductTransactionType.ExportFromRepairToStore:
                    await _service.ImportFromRepairToStoreAsync(returnedProductTransactionCreate);
                    break;
                case ReturnedProductTransactionType.ExportFromRepairToUtilize:
                    await _service.ImportFromRepairToUtilizeAsync(returnedProductTransactionCreate);
                    break;
                default:
                    break;
            }

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
