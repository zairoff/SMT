using SMT.Access.Repository.Base;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IRepairRepository : IBaseRepository<Repair>
    {
        Task<IEnumerable<Repair>> GetByAsync(Expression<Func<Repair, bool>> expression);
    }
}
