using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using System.Threading.Tasks;
using System;
using SMT.ViewModel.Dto.BoardReportDto;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardReportController : ControllerBase
    {
        private readonly IBoardReportService _service;

        public BoardReportController(IBoardReportService service)
        {
            _service = service;
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

        [HttpGet("GetByReader")]
        public async Task<IActionResult> GetByReader(int readerId, DateTime from, DateTime to)
        {
            var result = await _service.GetByReaderAsync(readerId, from, to);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BoardReportCreate reportCreate)
        {
            var result = await _service.AddAsync(reportCreate);

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
