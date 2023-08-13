using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using System.Threading.Tasks;
using System;
using SMT.ViewModel.Dto.PlanActivityDto;

namespace SMT.Api.Controllers
{
    public class PlanActivityController : BaseController
    {
        private readonly IPlanActivityService _service;

        public PlanActivityController(IPlanActivityService planService)
        {
            _service = planService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string shift)
        {
            var result = await _service.GetAllAsync(shift);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetAsync(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByLine")]
        public async Task<IActionResult> GetByLineId(int lineId, string shift)
        {
            var result = await _service.GetByLineId(lineId, shift);

            return Ok(result);
        }

        [HttpGet("GetByDate")]
        public async Task<IActionResult> GetByDate(string shift, DateTime date)
        {
            var result = await _service.GetByDate(shift, date);

            return Ok(result);
        }

        [HttpGet("GetByLineAndDate")]
        public async Task<IActionResult> GetByLineAndDate(int lineId, string shift, DateTime date)
        {
            var result = await _service.GetByLineAndDate(lineId, shift, date);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePlanActivity([FromBody] PlanActivityCreate planCreate)
        {  
            var result = await _service.AddAsync(planCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePlanActivity(int id, [FromBody] PlanActivityUpdate planUpdate)
        {
            var result = await _service.UpdateAsync(id, planUpdate);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePlanActivity(int id)
        {
            var result = await _service.DeleteAsync(id);

            return Ok(result);
        }
    }
}
