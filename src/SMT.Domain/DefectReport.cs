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

        public virtual Line Line { get; set; }

        public virtual Model Model { get; set; }

        public virtual Defect Defect { get; set; }
    }
}
