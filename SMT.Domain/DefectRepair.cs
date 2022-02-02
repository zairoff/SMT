using System;

namespace SMT.Domain
{
    public class DefectRepair
    {
        public int Id { get; set; }

        public int DefectReportId { get; set; }

        public DefectReport DefectReport { get; set; }

        public Guid EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public string Details { get; set; }

        public bool Status { get; set; }

        public DateTime Date { get; set; }
    }
}
