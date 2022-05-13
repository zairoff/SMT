namespace SMT.Domain
{
    public class ProductBrand
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }
    }
}
