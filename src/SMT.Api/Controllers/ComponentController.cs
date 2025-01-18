using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ComponentDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentController : ControllerBase
    {
        private readonly IComponentService _service;
        private readonly ILogger<ComponentController> _logger;

        public ComponentController(IComponentService service, ILogger<ComponentController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{page}/{pageSize}")]
        public async Task<IActionResult> Get([FromRoute] int page, [FromRoute] int pageSize)
        {
            var result = await _service.GetAsync(page, pageSize);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetAsync(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByPartNumber")]
        public async Task<IActionResult> GetByPartNumber(string partNumber)
        {
            var result = await _service.GetByPartNumberAsync(partNumber);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByRcode")]
        public async Task<IActionResult> GetByRcode(string rcode)
        {
            var result = await _service.GetByRcodeAsync(rcode);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByStorePlace")]
        public async Task<IActionResult> GetByStorePlace(string storePlace)
        {
            var result = await _service.GetByStorePlaceAsync(storePlace);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateComponent([FromBody] ComponentCreate componentCreate)
        {
            var result = await _service.AddAsync(componentCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPost("bulk")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BulkUpload([FromBody] List<ComponentCreate> components)
        {
            try
            {
                foreach (var component in components)
                {
                    var result = await _service.AddAsync(component);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.ToString());
            }

            return new OkResult();
        }

        [HttpPost("connect")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ComponentConnectToExistedOne([FromBody] List<ComponentConnect> components)
        {
            try
            {
                foreach (var component in components)
                {
                    var result = await _service.ConnectAsync(component.RCode, component.PartNumber);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new OkResult();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateComponent(int id, [FromBody] ComponentUpdate componentUpdate)
        {
            var result = await _service.UpdateAsync(id, componentUpdate);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var result = await _service.DeleteAsync(id);

            return Ok(result);
        }
    }
}
