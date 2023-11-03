using System;

namespace SMT.Domain.QA
{
    public class TesterPlan
    {
        public int Id { get; set; }

        public int TesterId { get; set; }

        public Tester Tester { get; set; }

        public DateTime Date { get; set; }
    }
}
