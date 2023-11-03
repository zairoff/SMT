using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.EmployeeDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool? isActive)
        {
            var result = await _service.GetAllAsync(isActive ?? true);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetAsync(id);

            return Ok(result);
        }

        [HttpGet("GetByDeparment")]
        public async Task<IActionResult> GetByDeparment(string departmentId, bool isActive)
        {
            var result = await _service.GetByDepartmentAsync(departmentId, isActive);

            return Ok(result);
        }


        [HttpGet("GetByStatus")]
        public async Task<IActionResult> GetByStatusAsync(bool isActive)
        {
            var result = await _service.GetByStatusAsync(isActive);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreate employeeCreate)
        {
            var result = await _service.AddAsync(employeeCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeUpdate employeeUpdate)
        {
            var result = await _service.UpdateAsync(id, employeeUpdate);

            return Ok(result);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeactivateEmployee(int id, bool isActive)
        {
            var result = await _service.DeactivateAsync(id, isActive);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var result = await _service.DeleteAsync(id);

            return Ok(result);
        }
    }
}
