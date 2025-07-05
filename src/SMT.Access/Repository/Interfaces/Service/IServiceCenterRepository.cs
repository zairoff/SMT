using SMT.Access.Repository.Base;
using SMT.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces.Service
{
    public interface IServiceCenterRepository : IBaseRepository<ServiceCenter>
    {
        Task<IEnumerable<ServiceCenter>> GetByAsync(Expression<Func<ServiceCenter, bool>> expression);
    }
}
