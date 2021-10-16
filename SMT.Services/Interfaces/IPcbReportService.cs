using SMT.Api.Common.Dto;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IPcbReportService
    {
        Task<IEnumerable<PcbReportResponse>> GetAllAsync();

        Task<PcbReportResponse> GetAsync(int id);

        Task<IEnumerable<PcbReportResponse>> GetByModelIdAsync(int modelId);

        Task<IEnumerable<PcbReportResponse>> GetByPositionIdAsync(int positionId);

        Task<IEnumerable<PcbReportResponse>> GetByDefectIdAsync(int defectId);

        Task<PcbReportResponse> AddAsync(PcbReportCreate reportCreate);

        Task<PcbReportResponse> UpdateAsync(int id, PcbReportUpdate reportUpdate);

        Task<PcbReportResponse> DeleteAsync(int id);
    }
}
