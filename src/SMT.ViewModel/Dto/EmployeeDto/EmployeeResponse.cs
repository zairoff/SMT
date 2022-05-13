using System;
using SMT.ViewModel.Dto.DepartmentDto;

namespace SMT.ViewModel.Dto.EmployeeDto
{
    public class EmployeeResponse
    {
        public int Id { get; set; }

        public string Passport { get; set; }

        public string DepartmentName { get; set; }

        public string Position { get; set; }

        public DepartmentResponse Department { get; set; }

        public string FullName { get; set; }

        public string ImageUrl { get; set; }

        public DateTime Birthday { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Details { get; set; }
    }
}
