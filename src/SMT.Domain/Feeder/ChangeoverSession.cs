using System;
using System.Collections.Generic;

namespace SMT.Domain.Feeder
{
    public class ChangeoverSession
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public Line Line { get; set; }

        public int FeederSetupId { get; set; }

        public FeederSetup FeederSetup { get; set; }

        public int? PreviousSetupId { get; set; }

        public FeederSetup PreviousSetup { get; set; }

        public ChangeoverStatus Status { get; set; } = ChangeoverStatus.InProgress;

        public string StartedBy { get; set; }

        public DateTime StartedAt { get; set; } = DateTime.Now;

        public string CompletedBy { get; set; }

        public DateTime? CompletedAt { get; set; }

        public List<ChangeoverItem> Items { get; set; } = new List<ChangeoverItem>();
    }
}
