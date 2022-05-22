using SMT.Access.Repository.Base;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IMachineRepairRepository : IBaseRepository<MachineRepair>
    {
        public Task<IEnumerable<MachineRepair>> GetByAsync(Expression<Func<MachineRepair, bool>> expression);
    }
}
