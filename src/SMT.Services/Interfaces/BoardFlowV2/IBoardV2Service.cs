using SMT.ViewModel.Dto.BoardV2Dto;
using SMT.ViewModel.Models.BoardFlowV2;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces.BoardFlowV2
{
    public interface IBoardV2Service
    {
        Task<BoardV2Response> ScanAsync(BoardV2Create create);
        Task<LineFlowSnapshot> GetLineSnapshotAsync(int lineId, DateTime from, DateTime to);
        Task<IEnumerable<LineFlowSnapshot>> GetAllLineSnapshotsAsync(DateTime from, DateTime to);
        Task<IEnumerable<BoardV2Response>> GetFlaggedAsync(int? lineId);
        Task<IEnumerable<BoardV2Response>> GetBoardsAtStationAsync(int readerId);
        Task<BoardHistoryResponse> GetHistoryAsync(string qrCode);
        Task<IEnumerable<RecentMovementResponse>> GetRecentMovementsAsync(DateTime date);
    }
}
