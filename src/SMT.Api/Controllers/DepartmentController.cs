using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.ViewModel.Dto.DepartmentDto;
using SMT.Services.Interfaces;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _service;

        public DepartmentController(IDepartmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByHierarchyId")]
        public async Task<IActionResult> GetByHierarchyId(string hierarchyId)
        {
            var result = await _service.GetByHierarchyId(hierarchyId);

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
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentCreate departmentCreate)
        {
            var result = await _service.AddAsync(departmentCreate);

            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentUpdate departmentUpdate)
        {
            var result = await _service.UpdateAsync(id, departmentUpdate);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var result = await _service.DeleteAsync(id);

            return Ok(result);
        }
    }
}
