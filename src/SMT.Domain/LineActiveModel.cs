using System;

namespace SMT.Domain
{
    // Tracks which Model a Line is currently running, set manually by a line lead
    // at changeover. Drives which set of ModelInstructionImage images each position's
    // Raspberry Pi display should show.
    public class LineActiveModel
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public virtual Line Line { get; set; }

        public int ModelId { get; set; }

        public virtual Model Model { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
