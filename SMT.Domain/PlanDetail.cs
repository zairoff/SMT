﻿namespace SMT.Domain
{
    public class PlanDetail
    {
        public int Id { get; set; }

        public int PlanId { get; set; }

        public string Details { get; set; }

        public Plan Plan { get; set; }
    }
}
