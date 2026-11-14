using System;
using System.Collections.Generic;

namespace SMT.Domain.Feeder
{
    // A versioned feeder program: which component goes to which slot for a model on a line.
    // Only one version per (Model, Line, Side) is Active; every import creates a new Draft version.
    public class FeederSetup
    {
        public int Id { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int LineId { get; set; }

        public Line Line { get; set; }

        public BoardSide Side { get; set; } = BoardSide.Top;

        public int Version { get; set; }

        public FeederSetupStatus Status { get; set; } = FeederSetupStatus.Draft;

        public string SourceFileName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string ActivatedBy { get; set; }

        public DateTime? ActivatedAt { get; set; }

        public List<FeederSetupItem> Items { get; set; } = new List<FeederSetupItem>();
    }
}
