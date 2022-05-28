using SMT.Domain;
using SMT.ViewModel.Dto.DefectDto;
using SMT.ViewModel.Dto.EmployeeDto;
using SMT.ViewModel.Dto.LineDto;
using SMT.ViewModel.Dto.ModelDto;
using SMT.ViewModel.Dto.PcbPositionDto;
using System;

namespace SMT.ViewModel.Dto.PcbReportDto
{
    public class PcbReportResponse
    {
        public int Id { get; set; }

        public EmployeeResponse Employee { get; set; }

        public LineResponse Line { get; set; }

        public ModelResponse Model { get; set; }

        public DefectResponse Defect { get; set; }

        public DateTime Date { get; set; }
    }
}
