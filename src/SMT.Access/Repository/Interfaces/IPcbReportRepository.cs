using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IPcbReportRepository
    {
        Task<IEnumerable<PcbReport>> GetByAsync(Expression<Func<PcbReport, bool>> expression);
        Task<int> CountAsync(Expression<Func<PcbReport, bool>> expression);
    }
}
