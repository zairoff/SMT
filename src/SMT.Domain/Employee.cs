using System;

namespace SMT.Domain
{
    public class Employee
    {
        public int Id { get; set; }

        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public string FullName { get; set; }

        public string ImagePath { get; set; }

        public string Phone { get; set; }

        public string Details { get; set; }

        public bool IsActive { get; set; }
    }
}
