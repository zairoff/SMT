using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Domain.Feeder;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Feeder
{
    public class MachineModuleRepository : BaseRepository<MachineModule>, IMachineModuleRepository
    {
        public MachineModuleRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<MachineModule> FindAsync(Expression<Func<MachineModule, bool>> expression)
        {
            return await DbSet.Include(m => m.LineMachine)
                .Include(m => m.Slots)
                .Where(expression)
                .FirstOrDefaultAsync();
        }
    }
}
