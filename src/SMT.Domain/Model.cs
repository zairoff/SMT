namespace SMT.Domain
{
    public class Model
    {
        public int Id { get; set; }

        public int ProductBrandId { get; set; }

        public virtual ProductBrand ProductBrand { get; set; }

        public string Name { get; set; }

        public string NameInBarcode { get; set; }

        public string SapCode { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
