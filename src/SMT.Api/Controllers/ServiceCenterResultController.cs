using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Service;
using SMT.ViewModel.Dto.ServiceCenterResultDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceCenterResultController : ControllerBase
    {
        private readonly IServiceCenterResultService _service;

        public ServiceCenterResultController(IServiceCenterResultService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] long messageId)
        {
            var result = await _service.GetAsync(messageId);

            return Ok(result);
        }

        [HttpPost("Cancel")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Cancel([FromBody] ServiceCenterResultStatus serviceCenterResultStatus)
        {
            var result = await _service.AddAsync(serviceCenterResultStatus, Domain.Service.ServiceStatus.Cancelled);

            return Ok(result);
        }

        [HttpPost("Start")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Start([FromBody] ServiceCenterResultStatus serviceCenterResultStatus)
        {
            var result = await _service.AddAsync(serviceCenterResultStatus, Domain.Service.ServiceStatus.Started);

            return Ok(result);
        }

        [HttpPost("Close")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Close([FromBody] ServiceCenterResultStatus serviceCenterResultStatus)
        {
            var result = await _service.AddAsync(serviceCenterResultStatus, Domain.Service.ServiceStatus.Closed);

            return Ok(result);
        }

        [HttpPost("ReOpen")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReOpen([FromBody] ServiceCenterResultStatus serviceCenterResultStatus)
        {
            var result = await _service.AddAsync(serviceCenterResultStatus, Domain.Service.ServiceStatus.ReOpened);

            return Ok(result);
        }
    }
}
