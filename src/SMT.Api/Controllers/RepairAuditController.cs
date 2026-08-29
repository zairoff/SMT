using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.RepairAuditDto;
using System;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    public class RepairAuditController : BaseController
    {
        private readonly IRepairAuditService _service;

        public RepairAuditController(IRepairAuditService service)
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
        [Route("GetByDateRange")]
        public async Task<IActionResult> GetByDateRange(DateTime from, DateTime to, int? modelId)
        {
            var result = await _service.GetByDateRangeAsync(from, to, modelId);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Scan([FromBody] RepairAuditCreate repairAuditCreate)
        {
            var result = await _service.ScanAsync(repairAuditCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
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
