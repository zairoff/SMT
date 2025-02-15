using SMT.ViewModel.Dto.BoardReportDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IBoardReportService
    {
        Task<BoardReportResponse> GetAsync(int id);

        Task<IEnumerable<BoardReportResponse>> GetByBarcodeAsync(string barcode);

        Task<IEnumerable<BoardReportResponse>> GetByReaderAsync(int readerId, DateTime from, DateTime to);

        Task<BoardReportResponse> AddAsync(BoardReportCreate boardReportCreate);

        Task<BoardReportResponse> DeleteAsync(int id);
    }
}
