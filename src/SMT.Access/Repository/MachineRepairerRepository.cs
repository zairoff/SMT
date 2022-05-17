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
    public class MachineRepairerRepository : BaseRepository<MachineRepairer>, IMachineRepairerRepository
    {
        public MachineRepairerRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<MachineRepairer> FindAsync(Expression<Func<MachineRepairer, bool>> expression)
        {
            return await DbSet.Include(e => e.Employee).Where(expression).FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<MachineRepairer>> GetAllAsync()
        {
            return await DbSet.Include(e => e.Employee).ToListAsync();
        }
    }
}
