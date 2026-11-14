using System.Collections.Generic;

namespace SMT.ViewModel.Dto.LineMachineDto
{
    public class MachineModuleResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public List<FeederSlotResponse> Slots { get; set; } = new List<FeederSlotResponse>();
    }
}
