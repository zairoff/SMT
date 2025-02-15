using SMT.Access.Repository.Base;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using SMT.Domain.BoardFlow;

namespace SMT.Access.Repository.Interfaces
{
    public interface IBoardReportRepository : IBaseRepository<BoardReport>
    {
        Task<IEnumerable<BoardReport>> GetByAsync(Expression<Func<BoardReport, bool>> expression);
    }
}
