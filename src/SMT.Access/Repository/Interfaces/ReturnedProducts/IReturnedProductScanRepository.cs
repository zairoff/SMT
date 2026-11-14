using SMT.Access.Repository.Base;
using SMT.Domain.ReturnedProducts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces.ReturnedProducts
{
    public interface IReturnedProductScanRepository : IBaseRepository<ReturnedProductScan>
    {
        Task<IEnumerable<ReturnedProductScan>> GetByAsync(Expression<Func<ReturnedProductScan, bool>> expression);
    }
}
