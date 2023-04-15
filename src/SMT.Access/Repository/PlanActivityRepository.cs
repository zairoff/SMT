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
    public class PlanActivityRepository : BaseRepository<PlanActivity>, IPlanActivityRepository
    {
        public PlanActivityRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<PlanActivity> FindAsync(Expression<Func<PlanActivity, bool>> expression)
        {
            return await DbSet.Where(expression)
                            .Include(m => m.Line)
                            .FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<PlanActivity>> GetAllAsync()
        {
            return await DbSet.Include(m => m.Line).ToListAsync();
        }

        public async Task<IEnumerable<PlanActivity>> GetByAsync(Expression<Func<PlanActivity, bool>> expression)
        {
            return await DbSet.Where(expression)
                            .Include(m => m.Line)
                            .ToListAsync();
        }
    }
}
