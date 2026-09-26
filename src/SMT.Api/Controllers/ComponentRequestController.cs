using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ComponentRequestDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    public class ComponentRequestController : BaseController
    {
        private readonly IComponentRequestService _service;

        public ComponentRequestController(IComponentRequestService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("open")]
        public async Task<IActionResult> GetOpen()
        {
            var result = await _service.GetOpenAsync();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ComponentRequestCreate componentRequestCreate)
        {
            var result = await _service.AddAsync(componentRequestCreate);

            return StatusCode(StatusCodes.Status201Created, result);
        }
    }
}
