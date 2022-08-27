using System;

namespace SMT.ViewModel.Dto.ReportDto
{
    public class ReportCreate
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public int DefectId { get; set; }

        public int ModelId { get; set; }

        public int EmployeeId { get; set; }

        public string Barcode { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
