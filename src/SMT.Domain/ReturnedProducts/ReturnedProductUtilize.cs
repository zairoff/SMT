namespace SMT.Domain.ReturnedProducts
{
    public class ReturnedProductUtilize
    {
        public int Id { get; set; }

        public int ReturnedProductTransactionId { get; set; }

        public virtual ReturnedProductTransaction ReturnedProductTransaction { get; set; }

        public string Barcode { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int Count { get; set; }
    }
}
