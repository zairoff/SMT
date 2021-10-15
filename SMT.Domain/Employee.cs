namespace SMT.Domain
{
    public class Employee
    {
        public int Id { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Image { get; set; }

        public string Birthday { get; set; }

        public string Phone { get; set; }
    }
}
