using System;

namespace SMT.Domain.BoardFlow.V2
{
    // Append-only scan-event log, kept for audit/history. Unlike V1's BoardReport,
    // it links to a BoardV2 row rather than being the only record of a board's state.
    public class BoardMovementV2
    {
        public int Id { get; set; }

        public int BoardId { get; set; }

        public BoardV2 Board { get; set; }

        public int QrReaderId { get; set; }

        public QrReaderV2 QrReader { get; set; }

        public DateTime DateTime { get; set; }

        public BoardMovementStatusV2 Status { get; set; }
    }
}
