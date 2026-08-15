using SMT.Access.Repository.Base;
using SMT.Domain.BoardFlow.V2;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.BoardFlowV2
{
    public interface IBoardV2Repository : IBaseRepository<BoardV2>
    {
        Task<BoardV2> FindByQrCodeAsync(string qrCode);

        Task<IEnumerable<BoardV2>> GetByAsync(Expression<Func<BoardV2, bool>> expression);

        Task<(IReadOnlyCollection<BoardV2> Items, int TotalCount)> GetFlaggedPagedAsync(int? lineId, int page, int pageSize);

        Task<IReadOnlyCollection<BoardStationCount>> GetStationCountsAsync(int lineId);

        Task<int> GetCompletedCountAsync(int lineId, DateTime from, DateTime to);

        Task<IReadOnlyCollection<BoardStationCount>> GetAllStationCountsAsync();

        Task<Dictionary<int, int>> GetAllCompletedCountsAsync(DateTime from, DateTime to);
    }
}
