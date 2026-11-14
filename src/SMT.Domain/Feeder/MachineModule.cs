using System.Collections.Generic;

namespace SMT.Domain.Feeder
{
    // FUJI module (M1..Mn) or Panasonic table - a group of feeder slots on the machine.
    public class MachineModule
    {
        public int Id { get; set; }

        public int LineMachineId { get; set; }

        public LineMachine LineMachine { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public List<FeederSlot> Slots { get; set; } = new List<FeederSlot>();
    }
}
