using SMT.ViewModel.Dto.ReportDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<ReportResponse>> GetAllAsync(string shift);

        Task<ReportResponse> GetAsync(int id);

        Task<IEnumerable<ReportResponse>> GetByModelAndLineIdAsync(int mdoelId, int lineId, string shift, DateTime date);

        Task<IEnumerable<ReportResponse>> GetByLineAndDefectAsync(int lineId, string line, string shift, DateTime from, DateTime to);

        Task<ReportResponse> AddAsync(ReportCreate reportCreate);

        Task<ReportResponse> DeleteAsync(int id);

        Task<IEnumerable<ReportResponse>> GetByAsync(int? modelId, int? lineId, string shift, DateTime from, DateTime to);

        Task<IEnumerable<ReportResponse>> GetByDateAsync(string shift, DateTime date);
    }
}
