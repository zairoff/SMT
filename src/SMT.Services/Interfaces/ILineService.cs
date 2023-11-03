using SMT.ViewModel.Dto.LineDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface ILineService
    {
        Task<IEnumerable<LineResponse>> GetAllAsync(bool? isActive);

        Task<LineResponse> GetAsync(int id);

        Task<LineResponse> GetByNameAsync(string name);

        Task<LineResponse> AddAsync(LineCreate lineCreate);

        Task<LineResponse> UpdateAsync(int id, LineUpdate lineUpdate);

        Task<LineResponse> DeleteAsync(int id);
    }
}
