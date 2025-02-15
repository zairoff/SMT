using SMT.Access.Repository.Base;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using SMT.Domain.BoardFlow;

namespace SMT.Access.Repository.Interfaces
{
    public interface IQrReaderRepository : IBaseRepository<QrReader>
    {
        Task<IEnumerable<QrReader>> GetByAsync(Expression<Func<QrReader, bool>> expression);
    }
}
