using SMT.Domain.BoardFlow.V2;
using SMT.ViewModel.Dto.LineDto;
using SMT.ViewModel.Dto.ModelDto;
using SMT.ViewModel.Dto.QrReaderV2Dto;
using System;

namespace SMT.ViewModel.Dto.BoardV2Dto
{
    public class BoardV2Response
    {
        public int Id { get; set; }

        public string QrCode { get; set; }

        public ModelResponse Model { get; set; }

        public int LineId { get; set; }

        public LineResponse Line { get; set; }

        public int? CurrentQrReaderId { get; set; }

        public QrReaderV2Response CurrentQrReader { get; set; }

        public BoardStatusV2 Status { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
