using SMT.ViewModel.Dto.EmployeeDto;

namespace SMT.ViewModel.Dto.LineOwnerDto
{
    public class LineOwnerResponse
    {
        public int Id { get; set; }

        public EmployeeResponse Employee { get; set; }
    }
}
