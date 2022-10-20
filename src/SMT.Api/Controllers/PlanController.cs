using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.EmployeeDto;
using SMT.ViewModel.Dto.PlanDto;

namespace SMT.Api.Controllers
{
    public class PlanController : BaseController
    {
        private readonly IPlanService _service;

        public PlanController(IPlanService planService)
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
            var result = await _service.GetByProductId(brandId);

            return Ok(result);
        }
        
        [HttpGet]
        [Route("GetByModel")]
        public async Task<IActionResult> GetByModelId(int modelId)
        {
            var result = await _service.GetByProductId(modelId);

            return Ok(result);
        }
        
        [HttpGet]
        [Route("GetByLine")]
        public async Task<IActionResult> GetByLineId(int lineId)
        {
            var result = await _service.GetByProductId(lineId);

            return Ok(result);
        }

        [HttpGet("GetByDate")]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            var result = await _service.GetByDate(date);

            return Ok(result);
        }

        // TODO: need to implement
        //[HttpGet]
        //[Route("GetBy")]
        //public async Task<IActionResult> GetBy(int productId, int brandId, int modelId, int lineId, DateTime from, DateTime to)
        //{
        //    var result = await _service.GetByAsync(productId, brandId, modelId, lineId, from, to);

        //    return Ok(result);
        //}

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReport([FromBody] PlanCreate planCreate)
        {
            var result = await _service.AddAsync(planCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReport(int id, [FromBody] PlanUpdate planUpdate)
        {
            var result = await _service.UpdateAsync(id, planUpdate);

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
