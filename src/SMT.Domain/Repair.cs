﻿using System;

namespace SMT.Domain
{
    public class Repair
    {
        public int Id { get; set; }

        public int ReportId { get; set; }

        public Report Report { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public string Action { get; set; }

        public string Condition { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
