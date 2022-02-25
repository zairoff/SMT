using SMT.Access.Repository.Base;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IProductBrandRepository : IBaseRepository<ProductBrand>
    {
        Task<IEnumerable<ProductBrand>> GetByAsync(Expression<Func<ProductBrand, bool>> expression);
    }
}
