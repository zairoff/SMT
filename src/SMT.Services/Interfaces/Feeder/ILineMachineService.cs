using SMT.ViewModel.Dto.LineMachineDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces.Feeder
{
    public interface ILineMachineService
    {
        Task<IEnumerable<LineMachineResponse>> GetAllAsync();
        Task<LineMachineResponse> GetByLineAsync(int lineId);
        Task<LineMachineResponse> SaveAsync(LineMachineSave save);
        Task<LineMachineResponse> AddModuleAsync(int lineId, MachineModuleSave save);
        Task<LineMachineResponse> UpdateModuleAsync(int moduleId, MachineModuleSave save);
        Task<LineMachineResponse> AddSlotsAsync(int moduleId, FeederSlotsCreate create);
        Task<LineMachineResponse> SetSlotActiveAsync(int slotId, bool isActive);
    }
}
