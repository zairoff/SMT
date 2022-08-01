using System;

namespace SMT.ViewModel.Dto.EmployeeDto
{
    public class EmployeeUpdate
    {
        public string ImagePath { get; set; }

        public int DepartmentId { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public string Details { get; set; }

        public bool IsActive { get; set; }
    }
}
