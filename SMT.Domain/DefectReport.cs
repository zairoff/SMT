using System;

namespace SMT.Domain
{
    public class DefectReport
    {
        public int Id { get; set; }

        public string Barcode { get; set; }

        public int LineId { get; set; }

        public int ModelId { get; set; }

        public int DefectId { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public Line Line { get; set; }

        public Model Model { get; set; }

        public Defect Defect { get; set; }
    }
}
