using SMT.ViewModel.Dto.LineDefectDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface ILineDefectService
    {
        Task<IEnumerable<LineDefectResponse>> GetAllAsync();

        Task<LineDefectResponse> GetAsync(int id);

        Task<IEnumerable<LineDefectResponse>> GetByLineIdAsync(int lineId);

        Task<LineDefectResponse> GetByLineAndDefectIdAsync(int lineId, int defectId);

        Task<LineDefectResponse> AddAsync(LineDefectCreate lineDefectCreate);

        Task<LineDefectResponse> UpdateAsync(int id, LineDefectUpdate lineDefectUpdate);

        Task<LineDefectResponse> DeleteAsync(int id);
    }
}
