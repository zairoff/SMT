namespace SMT.Domain
{
    public class ReadyProduct
    {
        public int Id { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int Count { get; set; }
    }
}
