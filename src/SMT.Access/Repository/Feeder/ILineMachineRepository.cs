using SMT.Access.Repository.Base;
using SMT.Domain.Feeder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Feeder
{
    public interface ILineMachineRepository : IBaseRepository<LineMachine>
    {
        // Machine with its modules and slots (active and inactive) - null when the line has no machine yet.
        Task<LineMachine> GetByLineAsync(int lineId);

        Task<IEnumerable<LineMachine>> GetAllWithLayoutAsync();
    }
}
