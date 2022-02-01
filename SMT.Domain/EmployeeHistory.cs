using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain
{
    public class EmployeeHistory
    {
        public int Id { get; set; }

        public Guid EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public DateTime Date { get; set; }

        public string Details { get; set; }
    }
}
