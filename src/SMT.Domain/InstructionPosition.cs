namespace SMT.Domain
{
    // A physical seat on a production Line where a staff member places components.
    // Order is used purely to sort positions for display, matching the physical layout.
    public class InstructionPosition
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public virtual Line Line { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }
    }
}
