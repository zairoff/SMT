using SMT.Domain.Feeder;
using System;
using System.Collections.Generic;

namespace SMT.ViewModel.Dto.FeederStationDto
{
    public class FeederScanResponse
    {
        public FeederScanResult Result { get; set; }

        public FeederNgReason? NgReason { get; set; }

        // The component the reel resolved to (null for an unknown reel).
        public int? ComponentId { get; set; }

        public string RCode { get; set; }

        public string Specification { get; set; }

        public string ReelPartNumber { get; set; }

        // The reel's part number differs from the one in the setup, but the RCode matches.
        public bool IsAlternate { get; set; }

        // Where this component goes in the setup (more than one when it's loaded in several feeders).
        public List<FeederSlotTarget> Targets { get; set; } = new List<FeederSlotTarget>();

        // The slot sticker that was scanned (verification step only; null when the sticker is unknown).
        public FeederSlotTarget ScannedSlot { get; set; }

        // For a wrong slot: what that slot should hold according to the setup.
        public string ScannedSlotExpectedRCode { get; set; }

        public DateTime ScannedAt { get; set; }
    }

    public class FeederSlotTarget
    {
        public int FeederSlotId { get; set; }

        public string SlotCode { get; set; }

        public string ModuleName { get; set; }

        public string SlotNumber { get; set; }
    }
}
