using SMT.Domain.Feeder;
using System.Collections.Generic;

namespace SMT.ViewModel.Dto.LineMachineDto
{
    public class LineMachineResponse
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public string LineName { get; set; }

        public string Name { get; set; }

        public MachineVendor Vendor { get; set; }

        public bool IsActive { get; set; }

        public List<MachineModuleResponse> Modules { get; set; } = new List<MachineModuleResponse>();
    }
}
