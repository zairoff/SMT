using SMT.ViewModel.Dto.ProductTransactionDto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace SMT.Services.Interfaces
{
    public interface IReadyProductTransactionService
    {
        Task<IEnumerable<ReadyProductTransactionResponse>> GetAllAsync();

        Task<IEnumerable<ReadyProductTransactionResponse>> GetByProductBrandAsync(int productBrandId);

        Task<IEnumerable<ReadyProductTransactionResponse>> GetByProductAsync(int productId);

        Task<IEnumerable<ReadyProductTransactionResponse>> GetByDateAsync(DateTime date, TransactionType transactionType);

        Task<IEnumerable<ReadyProductTransactionResponse>> GetByDateGroupByAsync(DateTime date, TransactionType transactionType);

        Task<IEnumerable<ReadyProductTransactionResponse>> GetByDateRangeAsync(DateTime from, DateTime to, TransactionType transactionType);

        Task<IEnumerable<ReadyProductTransactionResponse>> GetByDateRangeGroupByAsync(DateTime from, DateTime to, TransactionType transactionType);

        Task<IEnumerable<ReadyProductTransactionResponse>> GetBySapCodeDateRange(string sapCode, DateTime from, DateTime to, TransactionType transactionType);

        Task<ReadyProductTransactionResponse> DeleteTransactionAsync(int id);

        Task GroupByNotifyAsync();

        Task<ReadyProductTransactionResponse> ImportAsync(ReadyProductTransactionImport readyProductTransactionImport);

        Task<ReadyProductTransactionResponse> ExportAsync(ReadyProductTransactionExport readyProductTransactionExport);
    }
}
