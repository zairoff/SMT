using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Service;
using SMT.ViewModel.Dto.ServiceCenterRequestSenderDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceCenterRequestSenderController : ControllerBase
    {
        private readonly IRequestSenderService _service;

        public ServiceCenterRequestSenderController(IRequestSenderService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateServiceCenter([FromBody] RequestSenderCreate serviceCenterCreate)
        {
            var result = await _service.AddAsync(serviceCenterCreate);

            return Ok(result);
        }
    }
}
