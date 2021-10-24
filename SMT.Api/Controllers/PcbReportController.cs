using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Common.Dto.PcbReportDto;
using SMT.Common.Exceptions;
using SMT.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PcbReportController : ControllerBase
    {
        private readonly IPcbReportService _service;

        public PcbReportController(IPcbReportService service)
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
        [Route("GetByDefectId")]
        public async Task<IActionResult> GetByDefectId(int defectId)
        {
            var result = await _service.GetByDefectIdAsync(defectId);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByDateAndModelIdAsync")]
        public async Task<IActionResult> GetByDateAndModelIdAsync(int modelId, DateTime date)
        {
            var result = await _service.GetByDateAndModelIdAsync(modelId, date);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByPcbPositionId")]
        public async Task<IActionResult> GetByPcbPositionId(int pcdPositionId)
        {
            var result = await _service.GetByPositionIdAsync(pcdPositionId);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByModelId")]
        public async Task<IActionResult> GetByModelId(int modelId)
        {
            var result = await _service.GetByModelIdAsync(modelId);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReport([FromBody] PcbReportCreate reportCreate)
        {
            try
            {
                var result = await _service.AddAsync(reportCreate);

                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
                //return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReport(int id, [FromBody] PcbReportUpdate reportUpdate)
        {
            try
            {
                var result = await _service.UpdateAsync(id, reportUpdate);

                return Ok(result);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteReport(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);

                return Ok(result);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
