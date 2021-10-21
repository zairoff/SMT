using SMT.Common.Dto.ModelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IModelService
    {
        Task<IEnumerable<ModelResponse>> GetAllAsync();

        Task<ModelResponse> GetAsync(int id);

        Task<ModelResponse> GetByNameAsync(string name);

        Task<IEnumerable<ModelResponse>> GetByBrandIdAsync(int productId, int brandId);

        Task<ModelResponse> AddAsync(ModelCreate modelCreate);

        Task<ModelResponse> UpdateAsync(int id, ModelUpdate modelUpdate);

        Task<ModelResponse> DeleteAsync(int id);
    }
}
