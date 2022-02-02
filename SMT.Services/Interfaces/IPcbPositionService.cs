using SMT.Common.Dto.PcbPositionDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IPcbPositionService
    {
        Task<IEnumerable<PcbPositionResponse>> GetAllAsync();

        Task<PcbPositionResponse> GetAsync(int id);

        Task<PcbPositionResponse> GetByNameAsync(string name);

        Task<PcbPositionResponse> AddAsync(PcbPositionCreate positionCreate);

        Task<PcbPositionResponse> UpdateAsync(int id, PcbPositionUpdate positionUpdate);

        Task<PcbPositionResponse> DeleteAsync(int id);
    }
}
