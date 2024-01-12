using System;
using SMT.ViewModel.Dto.DepartmentDto;

namespace SMT.ViewModel.Dto.EmployeeDto
{
    public class EmployeeResponse
    {
        public int Id { get; set; }

        public DepartmentResponse Department { get; set; }

        public string FullName { get; set; }

        public string ImageUrl { get; set; }

        public string Phone { get; set; }

        public string Details { get; set; }

        public string Birthday { get; set; }

        public bool IsActive { get; set; }
    }
}
