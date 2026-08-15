using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.LineActiveModelDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineActiveModelController : BaseController
    {
        private readonly ILineActiveModelService _service;

        public LineActiveModelController(ILineActiveModelService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetByLine(int lineId)
        {
            var result = await _service.GetByLineAsync(lineId);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> SetActiveModel([FromBody] LineActiveModelSet activeModelSet)
        {
            var result = await _service.SetActiveModelAsync(activeModelSet);

            return Ok(result);
        }
    }
}
