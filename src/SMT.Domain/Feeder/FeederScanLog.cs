using System;

namespace SMT.Domain.Feeder
{
    // One reel + slot verification attempt, OK or NG. Raw codes are kept exactly as scanned so
    // NG scans of unknown reels/slots stay traceable.
    public class FeederScanLog
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public Line Line { get; set; }

        public FeederScanType ScanType { get; set; }

        public int? ChangeoverSessionId { get; set; }

        public ChangeoverSession ChangeoverSession { get; set; }

        public int? FeederSetupId { get; set; }

        public FeederSetup FeederSetup { get; set; }

        public int? FeederSlotId { get; set; }

        public FeederSlot FeederSlot { get; set; }

        public string RawReelCode { get; set; }

        public string RawSlotCode { get; set; }

        public string ReelPartNumber { get; set; }

        public int? ExpectedComponentId { get; set; }

        public Component ExpectedComponent { get; set; }

        public int? ScannedComponentId { get; set; }

        public Component ScannedComponent { get; set; }

        public FeederScanResult Result { get; set; }

        public FeederNgReason? NgReason { get; set; }

        public string Username { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
