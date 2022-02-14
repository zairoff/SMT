using SMT.Domain;
using System;

namespace SMT.ViewModel.Dto.PcbReportDto
{
    public class PcbReportResponse
    {
        public int Id { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int DefectId { get; set; }

        public Defect Defect { get; set; }

        public int PcbPositionId { get; set; }

        public PcbPosition PcbPosition { get; set; }

        public DateTime Date { get; set; }
    }
}
