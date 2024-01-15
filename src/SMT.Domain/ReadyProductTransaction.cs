using System;

namespace SMT.Domain
{
    public class ReadyProductTransaction
    {
        public int Id { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int Count { get; set; }

        public ReadyProductTransactionType Status { get; set; }

        public DateTime Date { get; set; }
    }
}
