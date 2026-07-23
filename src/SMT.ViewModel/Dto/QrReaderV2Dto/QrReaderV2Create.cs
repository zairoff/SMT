using System.Collections.Generic;

namespace SMT.ViewModel.Dto.QrReaderV2Dto
{
    public class QrReaderV2Create
    {
        public int LineId { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }

        // Empty/null means this is an entry station (nothing needs to precede it).
        // More than one entry means this station is a convergence point fed by
        // multiple upstream lines (e.g. a PCB line fed by two SMD lines).
        public List<int> PreviousReaderIds { get; set; } = new List<int>();
    }
}
