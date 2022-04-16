using SMT.Access.Repository.Base;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface ILineDefectRepository : IBaseRepository<LineDefect>
    {
        Task<IEnumerable<LineDefect>> GetByAsync(Expression<Func<LineDefect, bool>> expression);
    }
}
