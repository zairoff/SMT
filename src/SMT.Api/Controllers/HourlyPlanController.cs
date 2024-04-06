using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using System.Threading.Tasks;
using System;
using SMT.ViewModel.Dto.HourlyPlanDto;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HourlyPlanController : ControllerBase
    {
        private readonly IHourlyPlanService _service;

        public HourlyPlanController(IHourlyPlanService planService)
        {
            _service = planService;
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
        [Route("GetByProduct")]
        public async Task<IActionResult> GetByProductId(int productId)
        {
            var result = await _service.GetByProductId(productId);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByBrand")]
        public async Task<IActionResult> GetByBrandId(int brandId)
        {
            var result = await _service.GetByBrandId(brandId);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByModel")]
        public async Task<IActionResult> GetByModelId(int modelId)
        {
            var result = await _service.GetByModelId(modelId);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByLine")]
        public async Task<IActionResult> GetByLineId(int lineId)
        {
            var result = await _service.GetByLineId(lineId);

            return Ok(result);
        }

        [HttpGet("GetByDate")]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            var result = await _service.GetByDate(date);

            return Ok(result);
        }

        [HttpGet("GetByLineAndDate")]
        public async Task<IActionResult> GetByLineAndDate(int lineId, DateTime from, DateTime to)
        {
            var result = await _service.GetByLineAndDate(lineId, from, to);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePlan([FromBody] HourlyPlanCreate planCreate)
        {
            var result = await _service.AddAsync(planCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPost("Notify")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Notify()
        {
            await _service.NotifyAsync();

            return Ok();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePlan(int id, [FromBody] HourlyPlanUpdate planUpdate)
        {
            var result = await _service.UpdateAsync(id, planUpdate);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePlan(int id)
        {
            var result = await _service.DeleteAsync(id);

            return Ok(result);
        }
    }
}
