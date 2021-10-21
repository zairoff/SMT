using SMT.Common.Dto.BrandDto;
using SMT.Common.Dto.ProductDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Common.Dto.ModelDto
{
    public class ModelResponse
    {
        public int Id { get; set; }

        public BrandResponse Brand { get; set; }

        public ProductResponse Product { get; set; }

        public string Name { get; set; }
    }
}
