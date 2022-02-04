using SMT.Access.Repository.Base;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IModelRepository : IBaseRepository<Model>
    {
        Task<IEnumerable<Model>> GetByAsync(Expression<Func<Model, bool>> expression);
    }
}
