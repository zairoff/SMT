using Microsoft.AspNetCore.Http;
using System;

namespace SMT.ViewModel.Dto.EmployeeDto
{
    public class EmployeeUpdate
    {
        public string Passport { get; set; }

        public int DepartmentId { get; set; }

        public string FullName { get; set; }

        public IFormFile File { get; set; }

        public DateTime Birthday { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Details { get; set; }
    }
}
