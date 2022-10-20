using SMT.Access.Repository.Base;
using SMT.Domain;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace SMT.Access.Repository.Interfaces
{
    public interface IPlanRepository : IBaseRepository<Plan>
    {
        Task<IEnumerable<Plan>> GetByAsync(Expression<Func<Plan, bool>> expression);
    }
}
