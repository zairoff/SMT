using System.Collections.Generic;

namespace SMT.ViewModel.Dto.QrReaderV2Dto
{
    public class QrReaderV2Update
    {
        public int LineId { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }

        public List<int> PreviousReaderIds { get; set; } = new List<int>();
    }
}
