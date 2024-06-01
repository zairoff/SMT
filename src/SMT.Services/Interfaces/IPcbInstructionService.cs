using SMT.ViewModel.Dto.PcbInstructionDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IPcbInstructionService
    {
        Task<IEnumerable<string>> GetAsync();

        Task<string> GetAsync(int id);

        Task<string> GetByPositionAsync(int positionId);

        Task AddAsync(PcbInstructionCreate instructionCreate);
    }
}
