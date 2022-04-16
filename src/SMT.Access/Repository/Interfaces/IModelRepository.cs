using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IModelRepository
    {
        Task<IEnumerable<Model>> GetByAsync(Expression<Func<Model, bool>> expression);
    }
}
