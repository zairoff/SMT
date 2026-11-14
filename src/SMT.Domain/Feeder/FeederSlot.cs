using System;

namespace SMT.Domain.Feeder
{
    // A physical feeder position on a module. SlotCode is printed on the sticker next to the
    // slot and is generated once, so renaming a module doesn't invalidate stickers already on the machine.
    // The Current* fields hold what is loaded right now and are updated by verified scans.
    public class FeederSlot
    {
        public int Id { get; set; }

        public int MachineModuleId { get; set; }

        public MachineModule MachineModule { get; set; }

        public string Number { get; set; }

        public string SlotCode { get; set; }

        public bool IsActive { get; set; } = true;

        public int? CurrentComponentId { get; set; }

        public Component CurrentComponent { get; set; }

        public string CurrentReelCode { get; set; }

        public string LoadedBy { get; set; }

        public DateTime? LoadedAt { get; set; }
    }
}
