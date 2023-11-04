using SMT.ViewModel.Dto.ModelDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IModelService
    {
        Task<IEnumerable<ModelResponse>> GetAllAsync(bool? isActive);

        Task<ModelResponse> GetAsync(int id);

        Task<ModelResponse> GetByNameAsync(string name);

        Task<IEnumerable<ModelResponse>> GetByProductBrandId(int productBrandId, bool? isActive);

        Task<ModelResponse> AddAsync(ModelCreate modelCreate);

        Task<ModelResponse> UpdateAsync(int id, ModelUpdate modelUpdate);

        Task<ModelResponse> DeleteAsync(int id);
    }
}
