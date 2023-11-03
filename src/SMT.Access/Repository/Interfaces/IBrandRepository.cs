using SMT.Access.Repository.Base;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IBrandRepository : IBaseRepository<Brand>
    {
        Task<IEnumerable<Brand>> GetByAsync(Expression<Func<Brand, bool>> expression);
    }
}
