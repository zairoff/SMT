namespace SMT.Domain.BoardFlow.V2
{
    // A scanning station/checkpoint that belongs to a specific production Line.
    // Position orders stations within their own Line for display purposes only -
    // the actual flow topology (what must be scanned before this station) lives
    // in QrReaderV2Link, since a station can have more than one valid predecessor
    // (e.g. a PCB line fed by two upstream SMD lines).
    public class QrReaderV2
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public Line Line { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
