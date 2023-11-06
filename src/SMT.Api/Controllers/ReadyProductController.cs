using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ReadyProductDto;
using System;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReadyProductController : BaseController
    {
        private readonly IReadyProductService _service;

        public ReadyProductController(IReadyProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetAsync(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByEnterDate")]
        public async Task<IActionResult> GetByEnterDate(DateTime dateTime)
        {
            var result = await _service.GetByEnterDateAsync(dateTime);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByEnterDateRange")]
        public async Task<IActionResult> GetByEnterDateRange(DateTime from, DateTime to)
        {
            var result = await _service.GetByEnterDateRangeAsync(from, to);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByExitDate")]
        public async Task<IActionResult> GetByExitDate(DateTime dateTime)
        {
            var result = await _service.GetByExitDateAsync(dateTime);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByExitDateRange")]
        public async Task<IActionResult> GetByExitDateRange(DateTime from, DateTime to)
        {
            var result = await _service.GetByExitDateRangeAsync(from, to);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ReadyProductCreate readyProductCreate)
        {
            var result = await _service.AddAsync(readyProductCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] ReadyProductUpdate readyProductUpdate)
        {
            var result = await _service.UpdateAsync(id, readyProductUpdate);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            return Ok(result);
        }
    }
}
