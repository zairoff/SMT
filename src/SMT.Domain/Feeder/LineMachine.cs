using System.Collections.Generic;

namespace SMT.Domain.Feeder
{
    // The SMD placement machine of a line (one per line). Holds the module/slot layout
    // that feeder setups and slot stickers refer to.
    public class LineMachine
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public Line Line { get; set; }

        public string Name { get; set; }

        public MachineVendor Vendor { get; set; }

        // When true the operator must scan the slot sticker after the reel (verified placement).
        // Off by default: feeder positions are ~10 mm apart, so stickers only work with a scanner in
        // centering/picklist mode - without it the station only shows where the reel goes.
        public bool RequireSlotScan { get; set; }

        public bool IsActive { get; set; } = true;

        public List<MachineModule> Modules { get; set; } = new List<MachineModule>();
    }
}
