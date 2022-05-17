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


        Task<MachineRepairResponse> DeleteAsync(int id);
    }
}
