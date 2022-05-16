using SMT.ViewModel.Dto.EmployeeDto;
using SMT.ViewModel.Dto.ReportDto;
using System;

namespace SMT.ViewModel.Dto.RepairDto
{
    public class RepairResponse
    {
        public int Id { get; set; }

        public string Action { get; set; }

        public string Condition { get; set; }

        public DateTime Date { get; set; }

        public EmployeeResponse Employee { get; set; }

        public ReportResponse Report { get; set; }
    }
}
