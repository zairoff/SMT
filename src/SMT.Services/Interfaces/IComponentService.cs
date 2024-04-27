using SMT.ViewModel.Dto.ComponentDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IComponentService
    {
        Task<IEnumerable<ComponentResponse>> GetAllAsync();

        Task<ComponentResponse> GetAsync(int id);

        Task<ComponentResponse> GetByPartNumberAsync(string partNumber);

        Task<ComponentResponse> GetByStorePlaceAsync(string storePlaceNumber);

        Task<ComponentResponse> GetByRcodeAsync(string rcode);

        Task<ComponentResponse> AddAsync(ComponentCreate componentCreate);

        Task<ComponentResponse> UpdateAsync(int id, ComponentUpdate componentUpdate);

        Task<ComponentResponse> DeleteAsync(int id);
    }
}
