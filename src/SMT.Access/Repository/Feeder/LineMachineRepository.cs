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
    public class LineMachineRepository : BaseRepository<LineMachine>, ILineMachineRepository
    {
        public LineMachineRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<LineMachine> FindAsync(Expression<Func<LineMachine, bool>> expression)
        {
            return await WithLayout()
                .Where(expression)
                .FirstOrDefaultAsync();
        }

        public async Task<LineMachine> GetByLineAsync(int lineId)
        {
            return await FindAsync(m => m.LineId == lineId);
        }

        public async Task<IEnumerable<LineMachine>> GetAllWithLayoutAsync()
        {
            return await WithLayout()
                .OrderBy(m => m.LineId)
                .ToListAsync();
        }

        private IQueryable<LineMachine> WithLayout()
        {
            return DbSet.Include(m => m.Line)
                .Include(m => m.Modules)
                    .ThenInclude(module => module.Slots)
                        .ThenInclude(s => s.CurrentComponent);
        }
    }
}
