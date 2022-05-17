using System;

namespace SMT.Domain
{
    public class MachineRepair
    {
        public int Id { get; set; }

        public int MachineId { get; set; }

        public Machine Machine { get; set; }

        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public string Issue { get; set; }

        public string Action { get; set; }

        public DateTime? NotificationDate { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
