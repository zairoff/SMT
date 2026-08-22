using SMT.ViewModel.Dto.ComponentDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.GoogleSheets
{
    public interface IGoogleSheetsComponentImportService
    {
        Task<IEnumerable<ComponentBulkImportResult>> SyncFromSheetAsync();
    }
}
