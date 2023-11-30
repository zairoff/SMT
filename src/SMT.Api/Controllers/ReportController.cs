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
        [Route("GetByBarcode")]
        public async Task<IActionResult> GetByBarcode(string barcode)
        {
            var result = await _service.GetByBarcodeAsync(barcode);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetHistory")]
        public async Task<IActionResult> GetHistory(string barcode)
        {
            var results = await _service.GetHistoryAsync(barcode);

            return Ok(results);
        }

        [HttpGet("GetByDate")]
        public async Task<IActionResult> GetByDate(DateTime date, bool status)
        {
            var result = await _service.GetByDateAsync(date, status);

            return Ok(result);
        }

        [HttpGet("GetByUpdatedDate")]
        public async Task<IActionResult> GetByUpdatedDate(DateTime date, bool status)
        {
            var result = await _service.GetByUpdatedDate(date, status);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByModelIdAndLineId")]
        public async Task<IActionResult> GetByModelIdAndLineId(int modelId, int lineId, DateTime date, bool isClosed)
        {
            var result = await _service.GetByModelAndLineIdAsync(modelId, lineId, date, isClosed);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetBy")]
        public async Task<IActionResult> GetBy(int productId, int brandId, int modelId, int lineId, DateTime from, DateTime to)
        {
            var result = await _service.GetByAsync(productId, brandId, modelId, lineId, from, to);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByLineAndStatus")]
        public async Task<IActionResult> GetByLineAndStatus(int lineId, bool status, DateTime from, DateTime to)
        {
            var result = await _service.GetByLineAsync(lineId, status, from, to);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByLineAndDefect")]
        public async Task<IActionResult> GetByLineAndDefect(int lineId, string defectName, DateTime from, DateTime to)
        {
            var result = await _service.GetByLineAndDefectAsync(lineId, defectName, from, to);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReport([FromBody] ReportCreate reportCreate)
        {
            if (reportCreate.Barcode.Length != 7)
            {
                return new BadRequestObjectResult("Barcode noto'g'ri");
            }

            var result = await _service.AddAsync(reportCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReport(int id, [FromBody] ReportUpdate reportUpdate)
        {
            var result = await _service.UpdateAsync(id, reportUpdate);

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
