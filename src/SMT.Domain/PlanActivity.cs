using System;

namespace SMT.Domain
{
    public class PlanActivity
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public virtual Line Line { get; set; }

        public string Reason { get; set; }

        public string Act { get; set; }

        public string Responsible { get; set; }

        public string Status { get; set; }

        public DateTime Date { get; set; }

        public DateTime Expires { get; set; }
    }
}
