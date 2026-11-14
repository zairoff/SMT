using SMT.Domain.Feeder;
using System;

namespace SMT.ViewModel.Dto.FeederStationDto
{
    public class FeederScanLogResponse
    {
        public int Id { get; set; }

        public FeederScanType ScanType { get; set; }

        public FeederScanResult Result { get; set; }

        public FeederNgReason? NgReason { get; set; }

        public string RawReelCode { get; set; }

        public string RawSlotCode { get; set; }

        public string ModuleName { get; set; }

        public string SlotNumber { get; set; }

        public string ExpectedRCode { get; set; }

        public string ScannedRCode { get; set; }

        public string ScannedSpecification { get; set; }

        public string ReelPartNumber { get; set; }

        public string Username { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
