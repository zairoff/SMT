using SMT.Common.Dto.BrandDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<BrandResponse>> GetAllAsync();

        Task<BrandResponse> GetAsync(int id);

        Task<BrandResponse> GetByNameAsync(string name);

        Task<BrandResponse> AddAsync(BrandCreate brandCreate);

        Task<BrandResponse> UpdateAsync(int id, BrandUpdate brandUpdate);

        Task<BrandResponse> DeleteAsync(int id);
    }
}
