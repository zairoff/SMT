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
    public class MachineRepairRepository : BaseRepository<MachineRepair>, IMachineRepairRepository
    {
        public MachineRepairRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<MachineRepair> FindAsync(Expression<Func<MachineRepair, bool>> expression)
        {
            return await DbSet.Where(expression)
                            .Include(m => m.Employee)
                            .Include(m => m.Machine)
                            .FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<MachineRepair>> GetAllAsync()
        {
            return await DbSet.Include(m => m.Employee)
                            .Include(m => m.Machine)
                            .ToListAsync();
        }
    }
}
