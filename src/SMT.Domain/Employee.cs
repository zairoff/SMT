using System;

namespace SMT.Domain
{
    public class Employee
    {
        public int Id { get; set; }

        public string Passport { get; set; }

        public string Position { get; set; }

        public string DepartmentName { get; set; }

        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }

        public string FullName { get; set; }

        public string ImagePath { get; set; }

        public DateTime Birthday { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Details { get; set; }
    }
}
