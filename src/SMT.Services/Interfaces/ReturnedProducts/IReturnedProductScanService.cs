using SMT.Domain.ReturnedProducts;
using SMT.ViewModel.Dto.ReturnedProductScanDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces.ReturnedProducts
{
    public interface IReturnedProductScanService
    {
        Task<IEnumerable<ReturnedProductScanResponse>> GetByTransactionTypeAsync(ReturnedProductTransactionType transactionType);

        Task<ReturnedProductScanResponse> AddAsync(ReturnedProductScanCreate returnedProductScanCreate);

        Task<ReturnedProductScanResponse> DeleteAsync(int id);

        Task<IEnumerable<ReturnedProductScanResponse>> ClaimAsync(ReturnedProductScanClaim returnedProductScanClaim);
    }
}
