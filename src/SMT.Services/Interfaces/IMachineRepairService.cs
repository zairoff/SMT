using SMT.ViewModel.Dto.MachineRepairDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IMachineRepairService
    {
        Task<IEnumerable<MachineRepairResponse>> GetAllAsync(string shift);

        Task<MachineRepairResponse> GetAsync(int id);

        Task<MachineRepairResponse> AddAsync(MachineRepairCreate machineRepairCreate);

        Task<MachineRepairResponse> UpdateAsync(int id, MachineRepairUpdate machineRepairUpdate);

        Task<IEnumerable<MachineRepairResponse>> GetByMachineIdAsync(int id, string shift);

        Task<IEnumerable<MachineRepairResponse>> GetByMonthAsync(string shift, string date);

        Task<IEnumerable<MachineRepairResponse>> GetByMachineIdAndDateAsync(int machineId, string shift, string date);

        Task<MachineRepairResponse> DeleteAsync(int id);
    }
}
