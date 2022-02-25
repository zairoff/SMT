namespace SMT.Domain
{
    public class Defect
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public virtual Line Line { get; set; }

        public string Name { get; set; }
    }
}
