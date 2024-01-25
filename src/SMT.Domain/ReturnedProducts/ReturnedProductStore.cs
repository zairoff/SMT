﻿namespace SMT.Domain.ReturnedProducts
{
    public class ReturnedProductStore
    {
        public int Id { get; set; }

        public int ReturnedProductionTransactionId { get; set; }

        public ReturnedProductRepair ReturnedProductTransaction { get; set; }

        public string Barcode { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int Count { get; set; }
    }
}