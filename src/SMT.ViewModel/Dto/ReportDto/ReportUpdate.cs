using System;

namespace SMT.ViewModel.Dto.ReportDto
{
    public class ReportUpdate
    {
        public bool Status { get; set; }

        public string Employee { get; set; }

        public string Action { get; set; }

        public string Condition { get; set; }
    }
}
