using SMT.Access.Repository.Base;
using SMT.Domain.Feeder;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Feeder
{
    public interface IFeederSetupRepository : IBaseRepository<FeederSetup>
    {
        // Setup with items, their slots/modules and components.
        Task<FeederSetup> GetWithItemsAsync(int id);

        // Setup headers only (model/line, items without navigation) - for lists.
        Task<IEnumerable<FeederSetup>> GetByAsync(Expression<Func<FeederSetup, bool>> expression);

        Task<FeederSetup> GetActiveAsync(int modelId, int lineId, BoardSide side);

        Task<int> GetLatestVersionAsync(int modelId, int lineId, BoardSide side);
    }
}
