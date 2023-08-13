namespace SMT.Domain
{
    public class LineOwner
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }

        public int LineId { get; set; }

        public virtual Line Line { get; set; }
    }
}
