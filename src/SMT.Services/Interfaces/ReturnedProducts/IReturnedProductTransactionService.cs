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

        Task<ReturnedProductTransactionResponse> ImportFromFactoryAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate);

        Task<ReturnedProductTransactionResponse> ImportFromRepairAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate);

        Task<ReturnedProductTransactionResponse> ImportFromRepairToUtilizeAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate);

        Task<ReturnedProductTransactionResponse> ExportToUtilizeAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate);

        Task<ReturnedProductTransactionResponse> ExportToRepairAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate);

        Task<ReturnedProductTransactionResponse> ExportToFactoryAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate);

        Task<IEnumerable<ReturnedProductTransactionResponse>> GetByDateAsync(DateTime date, ReturnedProductTransactionType transactionType);

        Task<IEnumerable<ReturnedProductTransactionResponse>> GetByDateGroupByAsync(DateTime date, ReturnedProductTransactionType transactionType);

        Task<IEnumerable<ReturnedProductTransactionResponse>> GetByDateRangeAsync(DateTime from, DateTime to, ReturnedProductTransactionType transactionType);

        Task<IEnumerable<ReturnedProductTransactionResponse>> GetByDateRangeGroupByAsync(DateTime from, DateTime to, ReturnedProductTransactionType transactionType);

        Task<IEnumerable<ReturnedProductTransactionResponse>> GetStoreStateAsync();

        Task<IEnumerable<ReturnedProductTransactionResponse>> GetRepairStateAsync();

        Task<IEnumerable<ReturnedProductTransactionResponse>> GetUtilizeStateAsync();
    }
}
