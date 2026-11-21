using SMT.ViewModel.Dto.FeederStationDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces.Feeder
{
    public interface IFeederStationService
    {
        Task<FeederStationResponse> GetStationAsync(int lineId);

        // Step 1: reel scanned - where does it go? NG results (unknown reel, not in setup) are logged.
        // On lines that don't require the slot sticker this is the whole scan, so OK is logged too.
        Task<FeederScanResponse> LookupAsync(FeederReelScan scan, string username);

        // Step 2: reel + slot sticker - is it in the right place? Every result is logged; OK updates
        // what the slot currently holds.
        Task<FeederScanResponse> VerifyAsync(FeederReelScan scan, string username);

        Task<IEnumerable<FeederScanLogResponse>> GetRecentScansAsync(int lineId, int take);
    }
}
