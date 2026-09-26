using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository
{
    public class ComponentRequestItemRepository : BaseRepository<ComponentRequestItem>, IComponentRequestItemRepository
    {
        public ComponentRequestItemRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<ComponentRequestItem> FindAsync(Expression<Func<ComponentRequestItem, bool>> expression)
        {
            return await DbSet.Where(expression)
                            .Include(i => i.Component)
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ComponentRequestItem>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await DbSet.Where(i => ids.Contains(i.Id))
                            .Include(i => i.Component)
                            .ToListAsync();
        }
    }
}
