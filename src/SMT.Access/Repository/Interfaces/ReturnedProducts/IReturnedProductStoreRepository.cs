using SMT.Access.Repository.Base;
using SMT.Domain.ReturnedProducts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces.ReturnedProducts
{
    public interface IReturnedProductStoreRepository : IBaseRepository<ReturnedProductStore>
    {
        Task<int> FindSumAsync(Expression<Func<ReturnedProductStore, bool>> expression);

        Task<IEnumerable<ReturnedProductStore>> GetGroupByModelAsync();
    }
}
