using SMT.Domain.ReturnedProducts;

namespace SMT.ViewModel.Dto.ReturnedProductTransactionDto
{
    public class ReturnedProductTransactionCreate
    {
        public string Barcode { get; set; }

        public int ModelId { get; set; }

        public int Count { get; set; }

        public ReturnedProductTransactionType TransactionType { get; set; }
    }
}
