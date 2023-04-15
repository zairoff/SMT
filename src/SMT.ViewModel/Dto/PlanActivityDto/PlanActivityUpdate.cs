using System;

namespace SMT.ViewModel.Dto.PlanActivityDto
{
    public class PlanActivityUpdate
    {
        public int LineId { get; set; }

        public string Reason { get; set; }

        public string Act { get; set; }

        public string Responsible { get; set; }

        public string Status { get; set; }

        public DateTime Date { get; set; }

        public DateTime Expires { get; set; }
    }
}
