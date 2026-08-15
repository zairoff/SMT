using SMT.ViewModel.Dto.InstructionPositionDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IInstructionPositionService
    {
        Task<IEnumerable<InstructionPositionResponse>> GetAllAsync();

        Task<InstructionPositionResponse> GetAsync(int id);

        Task<IEnumerable<InstructionPositionResponse>> GetByLineAsync(int lineId);

        Task<InstructionPositionResponse> AddAsync(InstructionPositionCreate positionCreate);

        Task<InstructionPositionResponse> UpdateAsync(int id, InstructionPositionUpdate positionUpdate);

        Task<InstructionPositionResponse> DeleteAsync(int id);
    }
}
