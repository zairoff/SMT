using SMT.Access.Repository.Base;
using SMT.Domain.Feeder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Feeder
{
    public interface IFeederScanLogRepository : IBaseRepository<FeederScanLog>
    {
        // Latest scans of a line, newest first, with slot/module and components loaded.
        Task<IEnumerable<FeederScanLog>> GetRecentAsync(int lineId, int take);
    }
}
