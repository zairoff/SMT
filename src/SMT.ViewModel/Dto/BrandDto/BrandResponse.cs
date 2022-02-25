using SMT.Domain;

namespace SMT.ViewModel.Dto.BrandDto
{
    public class BrandResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static explicit operator BrandResponse(Brand brand)
        {
            return new BrandResponse { Name = brand.Name };
        }

        public static explicit operator Brand(BrandResponse brandResponse)
        {
            return new Brand { Name = brandResponse.Name };
        }
    }
}
