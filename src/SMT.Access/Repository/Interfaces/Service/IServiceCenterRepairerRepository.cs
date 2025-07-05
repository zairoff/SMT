using SMT.Access.Repository.Base;
using SMT.Domain.Service;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace SMT.Access.Repository.Interfaces.Service
{
    public interface IServiceCenterRepairerRepository : IBaseRepository<ServiceCenterRepairer>
    {
        Task<IEnumerable<ServiceCenterRepairer>> GetByAsync(Expression<Func<ServiceCenterRepairer, bool>> expression);
    }
}
