namespace SMT.Domain.Statics
{
    public class QualityDetailStaticsModel
    {
        public int LineId { get; set; }

        public string LineName { get; set; }

        public int ModelId { get; set; }

        public string ModelName { get; set; }

        public int Produced { get; set; }

        public int DefectCount { get; set; }

        public double Ftq { get; set; }
    }
}
