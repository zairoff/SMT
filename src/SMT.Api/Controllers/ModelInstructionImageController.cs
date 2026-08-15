using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ModelInstructionImageDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelInstructionImageController : BaseController
    {
        private readonly IModelInstructionImageService _service;

        public ModelInstructionImageController(IModelInstructionImageService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("GetByModel")]
        public async Task<IActionResult> GetByModel(int modelId)
        {
            var result = await _service.GetByModelAsync(modelId);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetByPosition")]
        public async Task<IActionResult> GetByPosition(int positionId)
        {
            var result = await _service.GetByPositionAsync(positionId);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetCurrentByPosition")]
        public async Task<IActionResult> GetCurrentByPosition(int positionId)
        {
            var result = await _service.GetCurrentByPositionAsync(positionId);

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrUpdate([FromBody] ModelInstructionImageCreate instructionImageCreate)
        {
            var result = await _service.AddOrUpdateAsync(instructionImageCreate);

            return Ok(result);
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
