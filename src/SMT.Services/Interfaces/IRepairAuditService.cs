using SMT.ViewModel.Dto.RepairAuditDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IRepairAuditService
    {
        Task<IEnumerable<RepairAuditResponse>> GetAllAsync();

        Task<RepairAuditResponse> GetAsync(int id);

        Task<IEnumerable<RepairAuditResponse>> GetByDateRangeAsync(DateTime from, DateTime to, int? modelId);

        Task<RepairAuditResponse> ScanAsync(RepairAuditCreate repairAuditCreate);

        Task<RepairAuditResponse> DeleteAsync(int id);

        Task RemoveByBarcodeAsync(string barcode);
    }
}
