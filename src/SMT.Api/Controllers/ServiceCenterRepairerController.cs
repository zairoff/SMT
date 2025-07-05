using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Service;
using SMT.ViewModel.Dto.ServiceCenterRepairerDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceCenterRepairerController : ControllerBase
    {
        private readonly IServiceCenterRepairerService _service;

        public ServiceCenterRepairerController(IServiceCenterRepairerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool? isActive)
        {
            var result = await _service.GetAllAsync(isActive ?? true);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetAsync(id);

            return Ok(result);
        }

        //[HttpGet]
        //[Route("GetByPhone")]
        //public async Task<IActionResult> GetByPhone(string phone)
        //{
        //    var result = await _service.GetByPhoneAsync(phone);

        //    return Ok(result);
        //}

        //[HttpGet]
        //[Route("GetByChat")]
        //public async Task<IActionResult> GetByChat(long chatId)
        //{
        //    var result = await _service.GetByChatIdAsync(chatId);

        //    return Ok(result);
        //}

        [HttpGet]
        [Route("GetByTelegram")]
        public async Task<IActionResult> GetByTelegram(long telegramId)
        {
            var result = await _service.GetByTelegramIdAsync(telegramId);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateServiceCenter([FromBody] ServiceCenterRepairerCreate serviceCenterCreate)
        {
            var result = await _service.AddAsync(serviceCenterCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteServiceCenter(int id)
        {
            var result = await _service.DeleteAsync(id);

            return Ok(result);
        }
    }
}
