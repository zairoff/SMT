namespace SMT.ViewModel.Models.BoardFlowV2
{
    // Live count of boards currently sitting at this checkpoint - derived from
    // BoardV2.CurrentQrReaderId/Status, not from re-diffing historical scan counts.
    public class StationSnapshot
    {
        public int ReaderId { get; set; }

        public string ReaderName { get; set; }

        public int Position { get; set; }

        public int InProgressCount { get; set; }

        public int FlaggedCount { get; set; }
    }
}
