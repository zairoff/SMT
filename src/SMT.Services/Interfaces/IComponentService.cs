using SMT.ViewModel.Dto.ComponentDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IComponentService
    {
        Task<IEnumerable<ComponentResponse>> GetAllAsync();

        Task<IEnumerable<ComponentResponse>> GetAsync(int page, int pageSize);

        Task<ComponentResponse> GetAsync(int id);

        Task<ComponentResponse> GetByPartNumberAsync(string partNumber);

        Task<ComponentResponse> GetByStorePlaceAsync(string storePlaceNumber);

        Task<ComponentResponse> GetByRcodeAsync(string rcode);

        Task<ComponentResponse> AddAsync(ComponentCreate componentCreate);

        Task<IEnumerable<ComponentBulkImportResult>> BulkAddAsync(List<ComponentCreate> components);

        Task<HashSet<string>> GetAllPartNumbersAsync();

        Task<ComponentResponse> ConnectAsync(string rcode, string partNumber);

        Task<ComponentResponse> UpdateAsync(int id, ComponentUpdate componentUpdate);

        Task<ComponentResponse> DeleteAsync(int id);
    }
}
