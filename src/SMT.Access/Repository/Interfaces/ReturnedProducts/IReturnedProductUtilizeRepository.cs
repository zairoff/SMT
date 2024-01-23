using SMT.Access.Repository.Base;
using SMT.Domain.ReturnedProducts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces.ReturnedProducts
{
    public interface IReturnedProductUtilizeRepository : IBaseRepository<ReturnedProductUtilize>
    {
        Task<int> FindSumAsync(Expression<Func<ReturnedProductUtilize, bool>> expression);

        Task<IEnumerable<ReturnedProductUtilize>> GetGroupByModelAsync();
    }
}
