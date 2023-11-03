using System;

namespace SMT.Domain.QA
{
    public class TesterProduce
    {
        public int Id { get; set; }

        public string Barcode { get; set; }

        public int TesterId { get; set; }

        public Tester Tester { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public bool Status { get; set; }

        public DateTime Started { get; set; }

        public DateTime Completed { get; set; }
    }
}
