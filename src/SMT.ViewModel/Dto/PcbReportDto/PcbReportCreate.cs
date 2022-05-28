using System;

namespace SMT.ViewModel.Dto.PcbReportDto
{
    public class PcbReportCreate
    {
        public int EmployeeId { get; set; }

        public int ModelId { get; set; }

        public int LineId { get; set; }

        public int DefectId { get; set; }

        public int PcbPositionId { get; set; }

        public static DateTime Date => DateTime.UtcNow;
    }
}
