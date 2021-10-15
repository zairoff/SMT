using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain
{
    public class Repair
    {
        public int Id { get; set; }

        public int ReportId { get; set; }

        public Report Report { get; set; }

        public int RepairerId { get; set; }

        public Repairer Repairer { get; set; }

        public string Action { get; set; }

        public DateTime Date { get; set; }
    }
}
