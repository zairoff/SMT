using SMT.Domain.ReturnedProducts;
using SMT.ViewModel.Dto.ReturnedProductTransactionDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces.ReturnedProducts
{
    public interface IReturnedProductTransactionService
    {
        Task<ReturnedProductTransactionResponse> DeleteTransactionAsync(int id);

        Task<ReturnedProductTransactionResponse> ImportFromFactoryToBufferAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate);

        Task<ReturnedProductTransactionResponse> ImportFromRepairToStoreAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate);

        Task<ReturnedProductTransactionResponse> ImportFromRepairToUtilizeAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate);

        Task<ReturnedProductTransactionResponse> ExportFromStoreToUtilizeAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate);

        Task<ReturnedProductTransactionResponse> ExportFromBufferToRepairAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate);

        Task<ReturnedProductTransactionResponse> ExportFromStoreToFactoryAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate);

        Task<IEnumerable<ReturnedProductTransactionResponse>> GetByDateAsync(DateTime date, ReturnedProductTransactionType transactionType);

        Task<IEnumerable<ReturnedProductTransactionResponse>> GetByDateGroupByAsync(DateTime date, ReturnedProductTransactionType transactionType);

        Task<IEnumerable<ReturnedProductTransactionResponse>> GetByDateRangeAsync(DateTime from, DateTime to, ReturnedProductTransactionType transactionType);

        Task<IEnumerable<ReturnedProductTransactionResponse>> GetByDateRangeGroupByAsync(DateTime from, DateTime to, ReturnedProductTransactionType transactionType);

        Task<IEnumerable<ReturnedProductTransactionResponse>> GetBufferStateAsync();

        Task<IEnumerable<ReturnedProductTransactionResponse>> GetStoreStateAsync();

        Task<IEnumerable<ReturnedProductTransactionResponse>> GetRepairStateAsync();

        Task<IEnumerable<ReturnedProductTransactionResponse>> GetUtilizeStateAsync();

        Task GroupByNotifyAsync();
    }
}
