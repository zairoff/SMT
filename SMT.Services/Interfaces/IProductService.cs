using SMT.Common.Dto.ProductDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetAllAsync();

        Task<ProductResponse> GetAsync(int id);

        Task<ProductResponse> GetByNameAsync(string name);

        Task<ProductResponse> AddAsync(ProductCreate productCreate);

        Task<ProductResponse> UpdateAsync(int id, ProductUpdate productUpdate);

        Task<ProductResponse> DeleteAsync(int id);
    }
}
