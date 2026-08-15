namespace SMT.ViewModel.Dto.ModelInstructionImageDto
{
    // Payload polled by a position's Raspberry Pi display. Always returned as a 200
    // with these flags set, rather than a 404, so the kiosk page can render a clear
    // placeholder instead of treating a normal "not configured yet" state as an error.
    public class CurrentInstructionResponse
    {
        public int PositionId { get; set; }

        public string PositionName { get; set; }

        public bool HasActiveModel { get; set; }

        public int? ModelId { get; set; }

        public string ModelName { get; set; }

        public bool HasImage { get; set; }

        public string ImagePath { get; set; }

        public string Message { get; set; }
    }
}
