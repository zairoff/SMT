﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.PlanDto;
using SMT.Domain;

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
        [Route("GetByModel")]
        public async Task<IActionResult> GetByModelId(int modelId, string shift)
        {
            var result = await _service.GetByModelId(modelId, shift);

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
        public async Task<IActionResult> GetByDate(DateTime date, string shift)
        {
            var result = await _service.GetByDate(date, shift);

            return Ok(result);
        }

        [HttpGet("GetByLineAndDate")]
        public async Task<IActionResult> GetByLineAndDate(int lineId, string shift, DateTime from, DateTime to)
        {
            var result = await _service.GetByLineAndDate(lineId, shift, from, to);

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
        public async Task<IActionResult> CreatePlan([FromBody] PlanCreate planCreate)
        {
            var result = await _service.AddAsync(planCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePlan(int id, [FromBody] PlanUpdate planUpdate)
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
