namespace SMT.Domain
{
    public class ProductBrand
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int BrandId { get; set; }

        public Brand Brand { get; set; }
    }
}
