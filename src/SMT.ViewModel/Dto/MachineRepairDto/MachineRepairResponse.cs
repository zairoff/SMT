using SMT.ViewModel.Dto.EmployeeDto;
using SMT.ViewModel.Dto.MachineDto;

namespace SMT.ViewModel.Dto.MachineRepairDto
{
    public class MachineRepairResponse
    {
        public int Id { get; set; }

        public MachineResponse Machine { get; set; }

        public EmployeeResponse Employee { get; set; }

        public string Issue { get; set; }

        public string Action { get; set; }

        public string NotificationDate { get; set; }

        public string CreatedDate { get; set; }
    }
}
