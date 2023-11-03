using SMT.ViewModel.Dto.MachineDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IMachineService
    {
        Task<IEnumerable<MachineResponse>> GetAllAsync(bool? isActive);

        Task<MachineResponse> GetAsync(int id);

        Task<MachineResponse> AddAsync(MachineCreate machineCreate);

        Task<MachineResponse> DeleteAsync(int id);
    }
}
