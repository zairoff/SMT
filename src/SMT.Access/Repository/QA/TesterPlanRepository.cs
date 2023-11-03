using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces.QA;
using SMT.Domain.QA;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace SMT.Access.Repository.QA
{
    public class TesterPlanRepository : BaseRepository<TesterPlan>, ITesterPlanRepository
    {
        public TesterPlanRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<TesterPlan> FindAsync(Expression<Func<TesterPlan, bool>> expression)
        {
            return await DbSet.Include(e => e.Tester).Where(expression).FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<TesterPlan>> GetAllAsync()
        {
            return await DbSet.Include(e => e.Tester).ToListAsync();
        }
    }
}
