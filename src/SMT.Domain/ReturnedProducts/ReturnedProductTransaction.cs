using System;

namespace SMT.Domain.ReturnedProducts
{
    public class ReturnedProductTransaction
    {
        public int Id { get; set; }

        public string Barcode { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int Count { get; set; }

        public ReturnedProductTransactionType TransactionType { get; set; }

        public DateTime Date { get; set; }
    }
}
