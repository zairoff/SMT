using SMT.ViewModel.Dto.BrandDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandResponse>> GetAllAsync(bool? isActive);

        Task<BrandResponse> GetAsync(int id);

        Task<BrandResponse> GetByNameAsync(string name);

        Task<BrandResponse> AddAsync(BrandCreate brandCreate);

        Task<BrandResponse> UpdateAsync(int id, BrandUpdate brandUpdate);

        Task<BrandResponse> DeleteAsync(int id);
    }
}
