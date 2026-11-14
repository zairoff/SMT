using SMT.Domain.ReturnedProducts;
using System.Collections.Generic;

namespace SMT.ViewModel.Dto.ReturnedProductScanDto
{
    public class ReturnedProductScanClaim
    {
        public ReturnedProductTransactionType TransactionType { get; set; }

        public IEnumerable<int> Ids { get; set; }
    }
}
