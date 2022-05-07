using System;

namespace SMT.ViewModel.Dto.ReportDto
{
    public class ReportCreate
    {
        public string Barcode { get; set; }

        public int LineId { get; set; }

        public int DefectId { get; set; }

        public int ModelId { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
