using SMT.ViewModel.Dto.ComponentRequestDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IComponentRequestService
    {
        Task<IEnumerable<ComponentRequestResponse>> GetAllAsync();

        Task<IEnumerable<ComponentRequestResponse>> GetOpenAsync();

        Task<ComponentRequestResponse> AddAsync(ComponentRequestCreate componentRequestCreate);
    }
}
