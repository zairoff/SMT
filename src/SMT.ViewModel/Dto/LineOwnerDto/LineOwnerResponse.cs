using SMT.ViewModel.Dto.EmployeeDto;
using SMT.ViewModel.Dto.LineDto;

namespace SMT.ViewModel.Dto.LineOwnerDto
{
    public class LineOwnerResponse
    {
        public int Id { get; set; }

        public EmployeeResponse Employee { get; set; }

        public LineResponse Line { get; set; }
    }
}
