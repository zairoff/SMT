namespace SMT.Domain
{
    public class Defect
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
