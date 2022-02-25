using SMT.Domain;

namespace SMT.ViewModel.Dto.BrandDto
{
    public class BrandCreate
    {
        public string Name { get; set; }

        public static explicit operator BrandCreate(Brand brand)
        {
            return new BrandCreate { Name = brand.Name };
        }

        public static explicit operator Brand(BrandCreate brandCreate)
        {
            return new Brand { Name = brandCreate.Name };
        }
    }
}
