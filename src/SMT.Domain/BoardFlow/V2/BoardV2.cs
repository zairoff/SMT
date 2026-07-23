using System;

namespace SMT.Domain.BoardFlow.V2
{
    // The single source of truth for "where is this physical board right now".
    // One row per QrCode; CurrentQrReaderId/Status are updated in place as the
    // board advances, instead of being re-derived from the movement log each time.
    public class BoardV2
    {
        public int Id { get; set; }

        public string QrCode { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int LineId { get; set; }

        public Line Line { get; set; }

        public int? CurrentQrReaderId { get; set; }

        public QrReaderV2 CurrentQrReader { get; set; }

        public BoardStatusV2 Status { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
