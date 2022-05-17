using SMT.ViewModel.Dto.EmployeeDto;

namespace SMT.ViewModel.Dto.MachineRepairerDto
{
    public class MachineRepairerResponse
    {
        public int Id { get; set; }

        public EmployeeResponse Employee { get; set; }
    }
}
