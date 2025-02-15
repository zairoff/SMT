using SMT.ViewModel.Dto.ModelDto;
using SMT.ViewModel.Dto.QrReaderDto;
using System;

namespace SMT.ViewModel.Dto.BoardReportDto
{
    public class BoardReportResponse
    {
        public int Id { get; set; }

        public string QrCode { get; set; }

        public ModelResponse Model { get; set; }

        public int QrReaderId { get; set; }

        public QrReaderResponse QrReader { get; set; }

        public DateTime DateTime { get; set; }
    }
}
