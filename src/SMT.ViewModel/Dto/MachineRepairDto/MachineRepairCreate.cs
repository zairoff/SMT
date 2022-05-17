using System;

namespace SMT.ViewModel.Dto.MachineRepairDto
{
    public class MachineRepairCreate
    {
        public int MachineId { get; set; }

        public int EmployeeId { get; set; }

        public string Issue { get; set; }

        public string Action { get; set; }

        public string NotificationDate { get; set; }

        public string CreatedDate { get; set; }
    }
}
