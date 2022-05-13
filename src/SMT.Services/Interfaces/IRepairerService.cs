using SMT.ViewModel.Dto.RepairerDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IRepairerService
    {
        Task<IEnumerable<RepairerResponse>> GetAllAsync();

        Task<RepairerResponse> GetAsync(int id);

        Task<RepairerResponse> AddAsync(RepairerCreate repairerCreate);

        Task<RepairerResponse> DeleteAsync(int id);
    }
}
