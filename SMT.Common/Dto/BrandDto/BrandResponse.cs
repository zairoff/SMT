using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Common.Dto.BrandDto
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
