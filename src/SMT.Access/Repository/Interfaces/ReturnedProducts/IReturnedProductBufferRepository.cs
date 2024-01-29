using SMT.Access.Repository.Base;
using SMT.Domain.ReturnedProducts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces.ReturnedProducts
{
    public interface IReturnedProductBufferRepository : IBaseRepository<ReturnedProductBufferZone>
    {
        Task<IEnumerable<ReturnedProductBufferZone>> GetGroupByModelAsync();

        Task<int> FindSumAsync(Expression<Func<ReturnedProductBufferZone, bool>> expression);
    }
}
