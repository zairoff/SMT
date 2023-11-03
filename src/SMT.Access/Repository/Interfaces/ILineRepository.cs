using SMT.Access.Repository.Base;
using SMT.Domain;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace SMT.Access.Repository.Interfaces
{
    public interface ILineRepository : IBaseRepository<Line>
    {
        Task<IEnumerable<Line>> GetByAsync(Expression<Func<Line, bool>> expression);
    }
}
