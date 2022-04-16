using SMT.ViewModel.Dto.DefectDto;
using SMT.ViewModel.Dto.LineDto;

namespace SMT.ViewModel.Dto.LineDefectDto
{
    public class LineDefectResponse
    {
        public int Id { get; set; }

        public LineResponse Line { get; set; }

        public DefectResponse Defect { get; set; }
    }
}
