using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Domain.Feeder;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Feeder
{
    public class FeederScanLogRepository : BaseRepository<FeederScanLog>, IFeederScanLogRepository
    {
        public FeederScanLogRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FeederScanLog>> GetRecentAsync(int lineId, int take)
        {
            return await DbSet.AsNoTracking()
                .Include(l => l.FeederSlot)
                    .ThenInclude(s => s.MachineModule)
                .Include(l => l.ExpectedComponent)
                .Include(l => l.ScannedComponent)
                .Where(l => l.LineId == lineId)
                .OrderByDescending(l => l.CreatedAt)
                .ThenByDescending(l => l.Id)
                .Take(take)
                .ToListAsync();
        }
    }
}
