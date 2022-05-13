using System;

namespace SMT.Domain
{
    public class PcbReport
    {
        public int Id { get; set; }

        public Guid EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public int LineId { get; set; }

        public virtual Line Line { get; set; }

        public int ModelId { get; set; }

        public virtual Model Model { get; set; }

        public int DefectId { get; set; }

        public virtual Defect Defect { get; set; }

        public int PositionId { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
