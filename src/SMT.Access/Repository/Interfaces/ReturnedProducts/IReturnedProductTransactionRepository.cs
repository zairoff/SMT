using SMT.Access.Repository.Base;
using SMT.Domain.ReturnedProducts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces.ReturnedProducts
{
    public interface IReturnedProductTransactionRepository : IBaseRepository<ReturnedProductTransaction>
    {
        Task<IEnumerable<ReturnedProductTransaction>> GetByAsync(Expression<Func<ReturnedProductTransaction, bool>> expression);

        Task<IEnumerable<ReturnedProductTransaction>> GetGroupByModelAsync(Expression<Func<ReturnedProductTransaction, bool>> expression);
    }
}
