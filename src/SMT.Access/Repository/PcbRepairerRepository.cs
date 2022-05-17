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
    public class PcbRepairerRepository : BaseRepository<PcbRepairer>, IPcbRepairerRepository
    {
        public PcbRepairerRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<PcbRepairer> FindAsync(Expression<Func<PcbRepairer, bool>> expression)
        {
            return await DbSet.Include(e => e.Employee).Where(expression).FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<PcbRepairer>> GetAllAsync()
        {
            return await DbSet.Include(e => e.Employee).ToListAsync();
        }
    }
}
