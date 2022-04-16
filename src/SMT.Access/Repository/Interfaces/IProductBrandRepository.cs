using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IProductBrandRepository
    {
        Task<IEnumerable<ProductBrand>> GetByAsync(Expression<Func<ProductBrand, bool>> expression);
    }
}
