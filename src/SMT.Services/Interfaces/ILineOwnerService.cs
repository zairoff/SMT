using SMT.ViewModel.Dto.LineOwnerDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface ILineOwnerService
    {
        Task<IEnumerable<LineOwnerResponse>> GetAllAsync();

        Task<LineOwnerResponse> GetAsync(int id);

        Task<LineOwnerResponse> AddAsync(LineOwnerCreate lineOwnerCreate);

        Task<LineOwnerResponse> DeleteAsync(int id);
    }
}
