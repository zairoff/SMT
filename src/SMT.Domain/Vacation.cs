using System;

namespace SMT.Domain
{
    public class Vacation
    {
        public int Id { get; set; }

        public Guid EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public string Details { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}
