using SMT.ViewModel.Dto.LineDto;
using System.Collections.Generic;

namespace SMT.ViewModel.Dto.QrReaderV2Dto
{
    public class QrReaderV2Response
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public LineResponse Line { get; set; }

        public string Name { get; set; }

        public int Position { get; set; }

        public bool IsActive { get; set; }

        public List<UpstreamReaderSummary> PreviousReaders { get; set; } = new List<UpstreamReaderSummary>();
    }
}
