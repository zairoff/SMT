using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Common.Dto.BrandDto;
using SMT.Common.Exceptions;
using SMT.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _service;

        public BrandController(IBrandService service)
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
        [Route("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _service.GetByNameAsync(name);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReport([FromBody] BrandCreate brandCreate)
        {
            try
            {
                var result = await _service.AddAsync(brandCreate);

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
        public async Task<IActionResult> UpdateReport(int id, [FromBody] BrandUpdate brandUpdate)
        {
            try
            {
                var result = await _service.UpdateAsync(id, brandUpdate);

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
