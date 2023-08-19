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
        public async Task<IActionResult> GroupByModel(string shift, DateTime from, DateTime to)
        {
            var result = await _staticsRepository.GroupByModelAsync(shift, from, to);

            return Ok(result);
        }

        [HttpGet("ByLine")]
        public async Task<IActionResult> GroupByLine(string shift, DateTime from, DateTime to)
        {
            var result = await _staticsRepository.GroupByLineAsync(shift, from, to);

            return Ok(result);
        }


        [HttpGet("ByDefect")]
        public async Task<IActionResult> GroupByDefect(string shift, DateTime from, DateTime to)
        {
            var result = await _staticsRepository.GroupByDefectAsync(shift, from, to);

            return Ok(result);
        }

        [HttpGet("DefectsByLine")]
        public async Task<IActionResult> DefectsByLine(int lineId, string shift, DateTime from, DateTime to)
        {
            var result = await _staticsRepository.GroupByDefectAsync(lineId, shift, from, to);

            return Ok(result);
        }

        [HttpGet("DefectCountByLine")]
        public async Task<IActionResult> DefectCountByLine(int lineId, string shift, DateTime from, DateTime to)
        {
            var result = await _staticsRepository.GroupByDefectAsync(lineId, "Defect", shift, from, to);

            return Ok(result);
        }
    }
}
