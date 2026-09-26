namespace SMT.ViewModel.Dto.ComponentRequestDto
{
    public class ComponentRequestItemResponse
    {
        public int Id { get; set; }

        public int ComponentId { get; set; }

        public string RCode { get; set; }

        public string PartNumber { get; set; }

        public string Status { get; set; }

        public string TransferredDate { get; set; }

        public double? DurationMinutes { get; set; }
    }
}
