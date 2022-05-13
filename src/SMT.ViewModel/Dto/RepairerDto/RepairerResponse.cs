using SMT.ViewModel.Dto.EmployeeDto;

namespace SMT.ViewModel.Dto.RepairerDto
{
    public class RepairerResponse
    {
        public int Id { get; set; }

        public EmployeeResponse Employee { get; set; }
    }
}
