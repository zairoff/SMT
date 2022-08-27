using SMT.ViewModel.Dto.ReportDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<ReportResponse>> GetAllAsync();

        Task<ReportResponse> GetAsync(int id);

        Task<ReportResponse> GetByBarcodeAsync(string barcode);

        Task<IEnumerable<ReportResponse>> GetByModelAndLineIdAsync(int mdoelId, int lineId, DateTime date);

        Task<ReportResponse> AddAsync(ReportCreate reportCreate);

        Task<ReportResponse> UpdateAsync(int id, ReportUpdate reportUpdate);

        Task<ReportResponse> DeleteAsync(int id);

        Task<IEnumerable<ReportResponse>> GetByAsync(int? productId,
                                                                int? brandId,
                                                                int? modelId,
                                                                int? lineId,
                                                                DateTime from,
                                                                DateTime to);

        Task<IEnumerable<ReportResponse>> GetByDateAsync(DateTime date);
    }
}
