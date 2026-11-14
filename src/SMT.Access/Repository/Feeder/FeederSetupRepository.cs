using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Domain.Feeder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Feeder
{
    public class FeederSetupRepository : BaseRepository<FeederSetup>, IFeederSetupRepository
    {
        public FeederSetupRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<FeederSetup> GetWithItemsAsync(int id)
        {
            return await WithItems()
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<FeederSetup>> GetByAsync(Expression<Func<FeederSetup, bool>> expression)
        {
            return await DbSet.Include(s => s.Model)
                .Include(s => s.Line)
                .Include(s => s.Items)
                .Where(expression)
                .OrderBy(s => s.LineId)
                .ThenBy(s => s.ModelId)
                .ThenBy(s => s.Side)
                .ThenByDescending(s => s.Version)
                .ToListAsync();
        }

        public async Task<FeederSetup> GetActiveAsync(int modelId, int lineId, BoardSide side)
        {
            return await WithItems()
                .Where(s => s.ModelId == modelId && s.LineId == lineId && s.Side == side
                    && s.Status == FeederSetupStatus.Active)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetLatestVersionAsync(int modelId, int lineId, BoardSide side)
        {
            return await DbSet.Where(s => s.ModelId == modelId && s.LineId == lineId && s.Side == side)
                .Select(s => (int?)s.Version)
                .MaxAsync() ?? 0;
        }

        private IQueryable<FeederSetup> WithItems()
        {
            return DbSet.Include(s => s.Model)
                .Include(s => s.Line)
                .Include(s => s.Items)
                    .ThenInclude(i => i.FeederSlot)
                        .ThenInclude(slot => slot.MachineModule)
                .Include(s => s.Items)
                    .ThenInclude(i => i.Component);
        }
    }
}
