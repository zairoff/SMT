using SMT.Access.Repository.Base;
using SMT.Domain.ReturnedProducts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces.ReturnedProducts
{
    public interface IReturnedProductRepairRepository : IBaseRepository<ReturnedProductRepair>
    {
        Task<IEnumerable<ReturnedProductRepair>> GetGroupByModelAsync();

        Task<int> FindSumAsync(Expression<Func<ReturnedProductRepair, bool>> expression);
    }
}
