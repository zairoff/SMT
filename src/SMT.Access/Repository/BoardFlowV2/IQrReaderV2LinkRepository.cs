using SMT.Access.Repository.Base;
using SMT.Domain.BoardFlow.V2;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Access.Repository.BoardFlowV2
{
    public interface IQrReaderV2LinkRepository : IBaseRepository<QrReaderV2Link>
    {
        // Upstream stations that must be passed before ToReader accepts a normal advance.
        Task<IEnumerable<QrReaderV2Link>> GetByToReaderAsync(int toReaderId);

        Task<bool> HasOutgoingAsync(int fromReaderId);
    }
}
