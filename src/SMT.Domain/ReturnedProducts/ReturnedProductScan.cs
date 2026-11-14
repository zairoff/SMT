using System;

namespace SMT.Domain.ReturnedProducts
{
    // A board scanned on /returned-product-export that has not been exported yet.
    // Kept server-side so the scan list can be continued from another device.
    public class ReturnedProductScan
    {
        public int Id { get; set; }

        public string Barcode { get; set; }

        public int? ModelId { get; set; }

        public Model Model { get; set; }

        public string SapCode { get; set; }

        public string SerialCode { get; set; }

        public int Count { get; set; }

        public ReturnedProductTransactionType TransactionType { get; set; }

        public DateTime Date { get; set; }
    }
}
