using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Common.Dto.BrandDto
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
