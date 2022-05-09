using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.ViewModel.Dto.EmployeeDto
{
    public class EmployeeCreate
    {
        public int DepartmentId { get; set; }

        public string FullName { get; set; }

        public string ImgBase64 { get; set; }

        public DateTime Birthday { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Details { get; set; }
    }
}
