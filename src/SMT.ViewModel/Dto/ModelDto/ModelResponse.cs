using SMT.ViewModel.Dto.ProductBrandDto;

namespace SMT.ViewModel.Dto.ModelDto
{
    public class ModelResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string NameInBarcode { get; set; }

        public string SapCode { get; set; }

        public bool Deactivated { get; set; }

        public ProductBrandResponse ProductBrand { get; set; }
    }
}
