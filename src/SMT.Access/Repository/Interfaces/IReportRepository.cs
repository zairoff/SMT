using SMT.Access.Repository.Base;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IReportRepository : IBaseRepository<Report>
    {
        public Task<IEnumerable<Report>> GetByAsync(Expression<Func<Report, bool>> expression);
    }
}
