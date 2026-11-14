using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces.Feeder;
using SMT.ViewModel.Dto.FeederStationDto;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    // Operator station at the line (no login - see FeederUser).
    public class FeederStationController : BaseController
    {
        private readonly IFeederStationService _service;

        public FeederStationController(IFeederStationService service)
        {
            _service = service;
        }

        [HttpGet("{lineId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStation(int lineId)
        {
            var result = await _service.GetStationAsync(lineId);

            return Ok(result);
        }

        [HttpGet("{lineId}/scans")]
        public async Task<IActionResult> GetRecentScans(int lineId, int take = 20)
        {
            var result = await _service.GetRecentScansAsync(lineId, take);

            return Ok(result);
        }

        [HttpPost("lookup")]
        public async Task<IActionResult> Lookup([FromBody] FeederReelScan scan)
        {
            var result = await _service.LookupAsync(scan, FeederUser.Username(User));

            return Ok(result);
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] FeederReelScan scan)
        {
            var result = await _service.VerifyAsync(scan, FeederUser.Username(User));

            return Ok(result);
        }
    }
}
