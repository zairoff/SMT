using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SMT.Services.Interfaces.BoardFlowV2;
using SMT.ViewModel.Dto.BoardV2Dto;
using System;
using System.Threading.Tasks;

namespace SMT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardV2Controller : ControllerBase
    {
        private readonly IBoardV2Service _service;

        public BoardV2Controller(IBoardV2Service service)
        {
            _service = service;
        }

        [HttpPost("Scan")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Scan([FromBody] BoardV2Create create)
        {
            var result = await _service.ScanAsync(create);

            return Ok(result);
        }

        [HttpGet("Snapshot")]
        public async Task<IActionResult> GetLineSnapshot(int lineId, DateTime from, DateTime to)
        {
            var result = await _service.GetLineSnapshotAsync(lineId, from, to);

            return Ok(result);
        }

        [HttpGet("Snapshots")]
        public async Task<IActionResult> GetAllLineSnapshots(DateTime from, DateTime to)
        {
            var result = await _service.GetAllLineSnapshotsAsync(from, to);

            return Ok(result);
        }

        [HttpGet("Flagged")]
        public async Task<IActionResult> GetFlagged(int? lineId, int page = 1, int pageSize = 10)
        {
            var result = await _service.GetFlaggedAsync(lineId, page, pageSize);

            return Ok(result);
        }

        [HttpGet("AtStation")]
        public async Task<IActionResult> GetBoardsAtStation(int readerId)
        {
            var result = await _service.GetBoardsAtStationAsync(readerId);

            return Ok(result);
        }

        [HttpGet("RecentMovements")]
        public async Task<IActionResult> GetRecentMovements(DateTime date)
        {
            var result = await _service.GetRecentMovementsAsync(date);

            return Ok(result);
        }

        [HttpGet("History")]
        public async Task<IActionResult> GetHistory(string qrCode)
        {
            var result = await _service.GetHistoryAsync(qrCode);

            return Ok(result);
        }
    }
}
