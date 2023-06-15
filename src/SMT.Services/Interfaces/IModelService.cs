using SMT.ViewModel.Dto.ModelDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IModelService
    {
        Task<IEnumerable<ModelResponse>> GetAllAsync();

        Task<ModelResponse> GetAsync(int id);

        Task<ModelResponse> GetByNameAsync(string name);

        Task<ModelResponse> AddAsync(ModelCreate modelCreate);

        Task<ModelResponse> UpdateAsync(int id, ModelUpdate modelUpdate);

        Task<ModelResponse> DeleteAsync(int id);
    }
}
