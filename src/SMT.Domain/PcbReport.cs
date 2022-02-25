using System;

namespace SMT.Domain
{
    public class PcbReport
    {
        public int Id { get; set; }

        public Guid EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public int LineId { get; set; }

        public Line Line { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int DefectId { get; set; }

        public Defect Defect { get; set; }

        public int PositionId { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
