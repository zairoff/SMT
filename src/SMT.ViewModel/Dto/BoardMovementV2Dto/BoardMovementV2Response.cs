using SMT.Domain.BoardFlow.V2;
using System;

namespace SMT.ViewModel.Dto.BoardMovementV2Dto
{
    public class BoardMovementV2Response
    {
        public int Id { get; set; }

        public int QrReaderId { get; set; }

        public string QrReaderName { get; set; }

        public DateTime DateTime { get; set; }

        public BoardMovementStatusV2 Status { get; set; }
    }
}
