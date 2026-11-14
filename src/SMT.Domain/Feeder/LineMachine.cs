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

        public bool IsActive { get; set; } = true;

        public List<MachineModule> Modules { get; set; } = new List<MachineModule>();
    }
}
