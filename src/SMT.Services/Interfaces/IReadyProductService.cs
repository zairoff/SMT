using SMT.ViewModel.Dto.ProductTransactionDto;
using SMT.ViewModel.Dto.ReadyProductDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IReadyProductService
    {
        Task<IEnumerable<ReadyProductResponse>> GetAllAsync();

        Task<IEnumerable<ReadyProductTransactionResponse>> GetByDateAsync(DateTime date, TransactionType transactionType);

        Task<IEnumerable<ReadyProductTransactionResponse>> GetByDateRangeAsync(DateTime from, DateTime to, TransactionType transactionType);

        Task<ReadyProductTransactionResponse> DeleteTransactionAsync(int id);

        Task<ReadyProductResponse> GetAsync(int id);

        Task<ReadyProductResponse> ImportAsync(ReadyProductCreate readyProductCreate);

        Task<ReadyProductResponse> ExportAsync(ReadyProductUpdate readyProductUpdate);
    }
}
