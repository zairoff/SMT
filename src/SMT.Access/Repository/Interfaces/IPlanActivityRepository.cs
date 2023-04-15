using SMT.Access.Repository.Base;
using SMT.Domain;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace SMT.Access.Repository.Interfaces
{
    public interface IPlanActivityRepository : IBaseRepository<PlanActivity>
    {
        Task<IEnumerable<PlanActivity>> GetByAsync(Expression<Func<PlanActivity, bool>> expression);
    }
}
