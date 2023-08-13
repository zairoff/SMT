using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ReportDto;
using System;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
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

        [HttpGet("GetByDate")]
        public async Task<IActionResult> GetByDate(string shift, DateTime date)
        {
            var result = await _service.GetByDateAsync(shift, date);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByModelIdAndLineId")]
        public async Task<IActionResult> GetByModelIdAndLineId(int modelId, int lineId, string shift, DateTime date)
        {
            var result = await _service.GetByModelAndLineIdAsync(modelId, lineId, shift, date);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetBy")]
        public async Task<IActionResult> GetBy(int modelId, int lineId, string shift, DateTime from, DateTime to)
        {
            var result = await _service.GetByAsync(modelId, lineId, shift, from, to);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReport([FromBody] ReportCreate reportCreate)
        {
            var result = await _service.AddAsync(reportCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
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
