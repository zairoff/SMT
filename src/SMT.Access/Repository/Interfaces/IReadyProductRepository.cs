using SMT.Access.Repository.Base;
using SMT.Domain;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace SMT.Access.Repository.Interfaces
{
    public interface IReadyProductRepository : IBaseRepository<ReadyProduct>
    {
        Task<IEnumerable<ReadyProduct>> GetByAsync(Expression<Func<ReadyProduct, bool>> expression);
    }
}
