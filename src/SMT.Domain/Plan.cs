using System;

namespace SMT.Domain
{
    public class Plan
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public int ModelId { get; set; }

        public int RequiredCount { get; set; }

        public int ProducedCount { get; set; }

        public DateTime Date { get; set; }

        public Line Line { get; set; }

        public Model Model { get; set; }
    }
}
