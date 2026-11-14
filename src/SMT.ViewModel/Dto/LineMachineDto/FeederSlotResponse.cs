using System;

namespace SMT.ViewModel.Dto.LineMachineDto
{
    public class FeederSlotResponse
    {
        public int Id { get; set; }

        public int MachineModuleId { get; set; }

        public string ModuleName { get; set; }

        public string Number { get; set; }

        public string SlotCode { get; set; }

        public bool IsActive { get; set; }

        public int? CurrentComponentId { get; set; }

        public string CurrentRCode { get; set; }

        public string CurrentSpecification { get; set; }

        public string CurrentReelCode { get; set; }

        public string LoadedBy { get; set; }

        public DateTime? LoadedAt { get; set; }
    }
}
