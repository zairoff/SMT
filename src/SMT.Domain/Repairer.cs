namespace SMT.Domain
{
    public class Repairer
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
