using SMT.ViewModel.Dto.ModelInstructionImageDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IModelInstructionImageService
    {
        Task<IEnumerable<ModelInstructionImageResponse>> GetByModelAsync(int modelId);

        Task<IEnumerable<ModelInstructionImageResponse>> GetByPositionAsync(int positionId);

        Task<ModelInstructionImageResponse> AddOrUpdateAsync(ModelInstructionImageCreate instructionImageCreate);

        Task<CurrentInstructionResponse> GetCurrentByPositionAsync(int positionId);

        Task<ModelInstructionImageResponse> DeleteAsync(int id);
    }
}
