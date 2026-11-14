namespace SMT.ViewModel.Dto.FeederStationDto
{
    // A scan at the operator station. SlotCode is empty for the lookup step (reel scanned, where
    // does it go?) and set for the verification step (reel + slot sticker).
    public class FeederReelScan
    {
        public int LineId { get; set; }

        public int SetupId { get; set; }

        public string ReelCode { get; set; }

        public string SlotCode { get; set; }
    }
}
