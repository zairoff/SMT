namespace SMT.Domain
{
    public class Barcode
    {
        public int Id { get; set; }

        public int ModelId { get; set; }

        public virtual Model Model { get; set; }

        public string Name { get; set; }
    }
}
