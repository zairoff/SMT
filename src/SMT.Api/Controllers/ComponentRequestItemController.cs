using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ComponentRequestDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    public class ComponentRequestItemController : BaseController
    {
        private readonly IComponentRequestItemService _service;

        public ComponentRequestItemController(IComponentRequestItemService service)
        {
            _service = service;
        }

        [HttpPut("{id}/not-found")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> MarkNotFound(int id)
        {
            var result = await _service.MarkNotFoundAsync(id);

            return Ok(result);
        }

        [HttpPost("transfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Transfer([FromBody] ComponentRequestItemTransfer transfer)
        {
            var result = await _service.TransferAsync(transfer);

            return Ok(result);
        }
    }
}
