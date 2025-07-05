using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Service;
using SMT.ViewModel.Dto.ServiceCenterRequestDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceCenterRequestController : ControllerBase
    {
        private readonly IServiceCenterRequestService _service;

        public ServiceCenterRequestController(IServiceCenterRequestService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateServiceCenter([FromBody] ServiceCenterRequestCreate serviceCenterCreate)
        {
            var result = await _service.AddAsync(serviceCenterCreate);

            return Ok(result);
        }
    }
}
