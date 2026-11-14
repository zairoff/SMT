using SMT.Domain.ReturnedProducts;

namespace SMT.ViewModel.Dto.ReturnedProductScanDto
{
    public class ReturnedProductScanCreate
    {
        public string Barcode { get; set; }

        public int? ModelId { get; set; }

        public string SapCode { get; set; }

        public string SerialCode { get; set; }

        public int Count { get; set; }

        public ReturnedProductTransactionType TransactionType { get; set; }
    }
}
