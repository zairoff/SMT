using SMT.ViewModel.Dto.MachineRepairDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IMachineRepairService
    {
        Task<IEnumerable<MachineRepairResponse>> GetAllAsync();

        Task<MachineRepairResponse> GetAsync(int id);

        Task<MachineRepairResponse> AddAsync(MachineRepairCreate machineRepairCreate);

        Task<MachineRepairResponse> UpdateAsync(int id, MachineRepairUpdate machineRepairUpdate);

        Task<IEnumerable<MachineRepairResponse>> GetByMachineIdAsync(int id);

        Task<IEnumerable<MachineRepairResponse>> GetByMonthAsync(string date);

        Task<IEnumerable<MachineRepairResponse>> GetByMachineIdAndDateAsync(int machineId, string date);

        Task<MachineRepairResponse> DeleteAsync(int id);
    }
}
