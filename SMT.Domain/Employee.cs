using System;

namespace SMT.Domain
{
    public class Employee
    {
        public Guid Id { get; set; }

        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public string FullName { get; set; }

        public string ImagePath { get; set; }

        public string Birthday { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Details { get; set; }

        public bool Status { get; set; }
    }
}
