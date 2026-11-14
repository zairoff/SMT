using SMT.Access.Repository.Base;
using SMT.Domain.Feeder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Feeder
{
    public interface IFeederSlotRepository : IBaseRepository<FeederSlot>
    {
        Task<FeederSlot> GetBySlotCodeAsync(string slotCode);

        Task<IEnumerable<FeederSlot>> GetByIdsAsync(IEnumerable<int> ids);

        Task<bool> SlotCodeExistsAsync(string slotCode);
    }
}
