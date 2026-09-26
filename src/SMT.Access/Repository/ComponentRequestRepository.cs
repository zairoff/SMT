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
    public class ComponentRequestRepository : BaseRepository<ComponentRequest>, IComponentRequestRepository
    {
        public ComponentRequestRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<ComponentRequest> FindAsync(Expression<Func<ComponentRequest, bool>> expression)
        {
            return await DbSet.Where(expression)
                            .Include(r => r.Line)
                            .Include(r => r.Items)
                                .ThenInclude(i => i.Component)
                            .FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<ComponentRequest>> GetAllAsync()
        {
            return await DbSet.Include(r => r.Line)
                            .Include(r => r.Items)
                                .ThenInclude(i => i.Component)
                            .OrderByDescending(r => r.CreatedDate)
                            .ToListAsync();
        }

        public async Task<IEnumerable<ComponentRequest>> GetOpenAsync()
        {
            return await DbSet.Include(r => r.Line)
                            .Include(r => r.Items)
                                .ThenInclude(i => i.Component)
                            .Where(r => r.Items.Any(i => i.Status == ComponentRequestItemStatus.Requested))
                            .OrderBy(r => r.CreatedDate)
                            .ToListAsync();
        }
    }
}
