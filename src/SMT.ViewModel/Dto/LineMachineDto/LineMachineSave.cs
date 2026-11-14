using SMT.Domain.Feeder;

namespace SMT.ViewModel.Dto.LineMachineDto
{
    // Creates the line's machine or updates it - a line has at most one.
    public class LineMachineSave
    {
        public int LineId { get; set; }

        public string Name { get; set; }

        public MachineVendor Vendor { get; set; }
    }
}
