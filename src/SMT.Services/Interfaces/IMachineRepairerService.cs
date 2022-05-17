using SMT.ViewModel.Dto.MachineRepairerDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IMachineRepairerService
    {
        Task<IEnumerable<MachineRepairerResponse>> GetAllAsync();

        Task<MachineRepairerResponse> GetAsync(int id);

        Task<MachineRepairerResponse> AddAsync(MachineRepairerCreate machineRepairerCreate);

        Task<MachineRepairerResponse> DeleteAsync(int id);
    }
}
