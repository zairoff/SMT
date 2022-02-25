using SMT.ViewModel.Dto.ProductDto;
using SMT.ViewModel.Dto.BrandDto;

namespace SMT.ViewModel.Dto.ProductBrandDto
{
    public class ProductBrandResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ProductResponse Product { get; set; }

        public BrandResponse Brand { get; set; }
    }
}
