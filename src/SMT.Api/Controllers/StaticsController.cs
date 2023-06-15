using Microsoft.AspNetCore.Mvc;
using SMT.Access.Repository.Statics;
using System;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaticsController : ControllerBase
    {
        private readonly IStaticsRepository _staticsRepository;

        public StaticsController(IStaticsRepository staticsRepository)
        {
            _staticsRepository = staticsRepository;
        }

        [HttpGet("ByModel")]
        public async Task<IActionResult> GroupByModel(DateTime from, DateTime to)
        {
            var result = await _staticsRepository.GroupByModelAsync(from, to);

            return Ok(result);
        }

        [HttpGet("ByLine")]
        public async Task<IActionResult> GroupByLine(DateTime from, DateTime to)
        {
            var result = await _staticsRepository.GroupByLineAsync(from, to);

            return Ok(result);
        }


        [HttpGet("ByDefect")]
        public async Task<IActionResult> GroupByDefect(DateTime from, DateTime to)
        {
            var result = await _staticsRepository.GroupByDefectAsync(from, to);

            return Ok(result);
        }

        [HttpGet("DefectsByLine")]
        public async Task<IActionResult> DefectsByLine(int lineId, DateTime from, DateTime to)
        {
            var result = await _staticsRepository.GroupByDefectAsync(lineId, from, to);

            return Ok(result);
        }

        [HttpGet("DefectCountByLine")]
        public async Task<IActionResult> DefectCountByLine(int lineId, DateTime from, DateTime to)
        {
            var result = await _staticsRepository.GroupByDefectAsync(lineId, "Defect", from, to);

            return Ok(result);
        }
    }
}
