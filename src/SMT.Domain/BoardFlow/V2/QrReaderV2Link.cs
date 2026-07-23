namespace SMT.Domain.BoardFlow.V2
{
    // A directed edge in the station flow graph: a board must have last been
    // seen at FromReader for ToReader to accept it as a normal advance.
    // A station with zero incoming links is an entry point (first station of
    // its own thread, e.g. each SMD line's start). A station with more than one
    // incoming link is a convergence point (e.g. the PCB line's entry station,
    // fed by both the Parmi and Jutze SMD lines).
    public class QrReaderV2Link
    {
        public int Id { get; set; }

        public int FromReaderId { get; set; }

        public QrReaderV2 FromReader { get; set; }

        public int ToReaderId { get; set; }

        public QrReaderV2 ToReader { get; set; }
    }
}
