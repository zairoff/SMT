namespace SMT.Domain
{
    public class Model
    {
        public int Id { get; set; }

        public int BrandId { get; set; }

        public Brand Brand { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public string Name { get; set; }
    }
}
