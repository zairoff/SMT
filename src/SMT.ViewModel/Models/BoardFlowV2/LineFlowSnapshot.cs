using System.Collections.Generic;

namespace SMT.ViewModel.Models.BoardFlowV2
{
    public class LineFlowSnapshot
    {
        public int LineId { get; set; }

        public string LineName { get; set; }

        public List<StationSnapshot> Stations { get; set; } = new List<StationSnapshot>();

        public int CompletedCount { get; set; }
    }
}
