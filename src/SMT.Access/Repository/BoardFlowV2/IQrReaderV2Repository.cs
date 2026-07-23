using SMT.Access.Repository.Base;
using SMT.Domain.BoardFlow.V2;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.BoardFlowV2
{
    public interface IQrReaderV2Repository : IBaseRepository<QrReaderV2>
    {
        Task<IEnumerable<QrReaderV2>> GetByAsync(Expression<Func<QrReaderV2, bool>> expression);
    }
}
