namespace SMT.Domain
{
    public class MachineRepairer
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
