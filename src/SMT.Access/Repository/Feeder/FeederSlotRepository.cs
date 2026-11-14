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
    public class FeederSlotRepository : BaseRepository<FeederSlot>, IFeederSlotRepository
    {
        public FeederSlotRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<FeederSlot> FindAsync(Expression<Func<FeederSlot, bool>> expression)
        {
            return await WithModule()
                .Where(expression)
                .FirstOrDefaultAsync();
        }

        public async Task<FeederSlot> GetBySlotCodeAsync(string slotCode)
        {
            return await FindAsync(s => s.SlotCode == slotCode);
        }

        public async Task<IEnumerable<FeederSlot>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var idList = ids.ToList();

            return await WithModule()
                .Where(s => idList.Contains(s.Id))
                .ToListAsync();
        }

        public async Task<bool> SlotCodeExistsAsync(string slotCode)
        {
            return await DbSet.AnyAsync(s => s.SlotCode == slotCode);
        }

        private IQueryable<FeederSlot> WithModule()
        {
            return DbSet.Include(s => s.MachineModule)
                    .ThenInclude(m => m.LineMachine)
                .Include(s => s.CurrentComponent);
        }
    }
}
