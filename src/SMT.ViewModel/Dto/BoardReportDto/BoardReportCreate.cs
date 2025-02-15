namespace SMT.ViewModel.Dto.BoardReportDto
{
    public class BoardReportCreate
    {
        public string QrCode { get; set; }

        public int QrReaderId { get; set; }

        public int QrReaderPositionId { get; set; }

        public int QrReaderPreviousPositionId { get; set; }
    }
}
