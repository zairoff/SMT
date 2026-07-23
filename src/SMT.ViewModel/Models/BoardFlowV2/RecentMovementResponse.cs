using SMT.Domain.BoardFlow.V2;
using System;

namespace SMT.ViewModel.Models.BoardFlowV2
{
    // One row in a scan report log - a specific scan event, not a board's
    // overall current state (which may have moved on since).
    public class RecentMovementResponse
    {
        public int Id { get; set; }

        public string QrCode { get; set; }

        public string ModelName { get; set; }

        public int QrReaderId { get; set; }

        public string QrReaderName { get; set; }

        public string LineName { get; set; }

        public DateTime DateTime { get; set; }

        public BoardMovementStatusV2 Status { get; set; }
    }
}
