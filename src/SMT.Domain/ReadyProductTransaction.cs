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

    public enum ReadyProductTransactionType
    {
        All = 0,
        Import = 1,
        Export = 2,
        Deleted = 3,
    }
}
