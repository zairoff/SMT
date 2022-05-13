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
    public class RepairerRepository : BaseRepository<Repairer>, IRepairerRepository
    {
        public RepairerRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<Repairer> FindAsync(Expression<Func<Repairer, bool>> expression)
        {
            return await DbSet.Include(e => e.Employee).Where(expression).FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<Repairer>> GetAllAsync()
        {
            return await DbSet.Include(e => e.Employee).ToListAsync();
        }
    }
}
