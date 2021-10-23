using SMT.Common.Dto.BrandDto;
using SMT.Common.Dto.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Common.Dto.ProductBrandDto
{
    public class ProductBrandResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ProductResponse Product { get; set; }

        public BrandResponse Brand { get; set; }
    }
}
