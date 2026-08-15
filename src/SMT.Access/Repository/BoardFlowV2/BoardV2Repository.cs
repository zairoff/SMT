using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Domain.BoardFlow.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.BoardFlowV2
{
    public class BoardV2Repository : BaseRepository<BoardV2>, IBoardV2Repository
    {
        public BoardV2Repository(AppDbContext context) : base(context)
        {
        }

        public async override Task<BoardV2> FindAsync(Expression<Func<BoardV2, bool>> expression)
        {
            return await DbSet.Include(b => b.Model)
                .Include(b => b.Line)
                .Include(b => b.CurrentQrReader)
                    .ThenInclude(r => r.Line)
                .Where(expression)
                .FirstOrDefaultAsync();
        }

        public Task<BoardV2> FindByQrCodeAsync(string qrCode)
        {
            return FindAsync(b => b.QrCode == qrCode && b.Status != BoardStatusV2.Deleted);
        }

        public async Task<IEnumerable<BoardV2>> GetByAsync(Expression<Func<BoardV2, bool>> expression)
        {
            return await DbSet.Where(expression)
                .Include(b => b.Model)
                .Include(b => b.Line)
                .Include(b => b.CurrentQrReader)
                .OrderByDescending(b => b.UpdatedAt)
                .ToListAsync();
        }

        public async Task<(IReadOnlyCollection<BoardV2> Items, int TotalCount)> GetFlaggedPagedAsync(int? lineId, int page, int pageSize)
        {
            var query = DbSet.Where(b =>
                b.Status == BoardStatusV2.Flagged && (lineId == null || b.LineId == lineId));

            var totalCount = await query.CountAsync();

            var items = await query
                .Include(b => b.Model)
                .Include(b => b.Line)
                .Include(b => b.CurrentQrReader)
                .OrderByDescending(b => b.UpdatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IReadOnlyCollection<BoardStationCount>> GetStationCountsAsync(int lineId)
        {
            return await DbSet
                .Where(b => b.LineId == lineId
                    && b.CurrentQrReaderId != null
                    && (b.Status == BoardStatusV2.InProgress || b.Status == BoardStatusV2.Flagged))
                .GroupBy(b => new { ReaderId = b.CurrentQrReaderId.Value, b.Status })
                .Select(g => new BoardStationCount
                {
                    ReaderId = g.Key.ReaderId,
                    Status = g.Key.Status,
                    Count = g.Count()
                })
                .ToListAsync();
        }

        public async Task<int> GetCompletedCountAsync(int lineId, DateTime from, DateTime to)
        {
            return await DbSet.CountAsync(b => b.LineId == lineId
                && b.Status == BoardStatusV2.Completed
                && b.CompletedAt != null
                && b.CompletedAt.Value.Date >= from.Date
                && b.CompletedAt.Value.Date <= to.Date);
        }

        public async Task<IReadOnlyCollection<BoardStationCount>> GetAllStationCountsAsync()
        {
            return await DbSet
                .Where(b => b.CurrentQrReaderId != null
                    && (b.Status == BoardStatusV2.InProgress || b.Status == BoardStatusV2.Flagged))
                .GroupBy(b => new { ReaderId = b.CurrentQrReaderId.Value, b.Status })
                .Select(g => new BoardStationCount
                {
                    ReaderId = g.Key.ReaderId,
                    Status = g.Key.Status,
                    Count = g.Count()
                })
                .ToListAsync();
        }

        public async Task<Dictionary<int, int>> GetAllCompletedCountsAsync(DateTime from, DateTime to)
        {
            return await DbSet
                .Where(b => b.Status == BoardStatusV2.Completed
                    && b.CompletedAt != null
                    && b.CompletedAt.Value.Date >= from.Date
                    && b.CompletedAt.Value.Date <= to.Date)
                .GroupBy(b => b.LineId)
                .Select(g => new { LineId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.LineId, x => x.Count);
        }
    }
}
