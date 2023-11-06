using System;

namespace SMT.Domain
{
    public class ReadyProduct
    {
        public int Id { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int Count { get; set; }

        public bool Inside { get; set; }

        public DateTime Enter { get; set; }

        public DateTime Exit { get; set; }
    }
}
