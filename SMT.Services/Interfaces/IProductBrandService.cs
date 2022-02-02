using SMT.Common.Dto.ProductBrandDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IProductBrandService
    {
        Task<IEnumerable<ProductBrandResponse>> GetAllAsync();

        Task<ProductBrandResponse> GetAsync(int id);

        Task<IEnumerable<ProductBrandResponse>> GetByProductIdAsync(int productId);

        Task<ProductBrandResponse> AddAsync(ProductBrandCreate productBrandCreate);

        Task<ProductBrandResponse> UpdateAsync(int id, ProductBrandUpdate productBrandUpdate);

        Task<ProductBrandResponse> DeleteAsync(int id);
    }
}
