using Microsoft.AspNetCore.Mvc;
using SMT.Domain.ReturnedProducts;
using SMT.Services.Interfaces.ReturnedProducts;
using SMT.ViewModel.Dto.ReturnedProductScanDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnedProductScanController : ControllerBase
    {
        private readonly IReturnedProductScanService _service;

        public ReturnedProductScanController(IReturnedProductScanService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(ReturnedProductTransactionType transactionType)
        {
            var result = await _service.GetByTransactionTypeAsync(transactionType);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ReturnedProductScanCreate returnedProductScanCreate)
        {
            var result = await _service.AddAsync(returnedProductScanCreate);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            return Ok(result);
        }

        [HttpPost]
        [Route("Claim")]
        public async Task<IActionResult> Claim([FromBody] ReturnedProductScanClaim returnedProductScanClaim)
        {
            var result = await _service.ClaimAsync(returnedProductScanClaim);

            return Ok(result);
        }
    }
}
