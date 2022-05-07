using SMT.ViewModel.Dto.DefectDto;
using SMT.ViewModel.Dto.EmployeeDto;
using SMT.ViewModel.Dto.LineDto;
using SMT.ViewModel.Dto.ModelDto;
using System;

namespace SMT.ViewModel.Dto.ReportDto
{
    public class ReportResponse
    {
        public int Id { get; set; }

        public LineResponse Line  { get; set; }

        public DefectResponse Defect { get; set; }

        public ModelResponse Model { get; set; }

        public string Barcode { get; set; }

        public DateTime Date { get; set; }
    }
}
