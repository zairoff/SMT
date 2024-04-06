using System;

namespace SMT.Domain
{
    public class HourlyPlan
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public Line Line { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int Plan { get; set; }

        public int Produced { get; set; }

        public DateTime Time { get; set; }
    }
}
