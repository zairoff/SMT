using System;

namespace SMT.ViewModel.Dto.ReportDto
{
    public class ReportUpdate
    {
        public int EmployeeId { get; set; }

        public string Action { get; set; }

        public bool Status { get; set; }

        public DateTime Updated { get; set; }
    }
}
