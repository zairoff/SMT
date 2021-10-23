using AutoMapper;
using SMT.Common.Dto.PcbReportDto;
using SMT.Common.Dto.BrandDto;
using SMT.Common.Dto.ProductDto;
using SMT.Common.Dto.ModelDto;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMT.Common.Dto.PcbPositionDto;
using SMT.Common.Dto.ProductBrandDto;
using SMT.Common.Dto.DefectDto;

namespace SMT.Services.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<PcbReport, PcbReportResponse>();
            CreateMap<Product, ProductResponse>();
            CreateMap<Brand, BrandResponse>();
            CreateMap<Model, ModelResponse>();
            CreateMap<PcbPosition, PcbPositionResponse>();
            CreateMap<ProductBrand, ProductBrandResponse>()
                    .ForPath(dst => dst.Name, src => src.MapFrom(s => s.Brand.Name));

            CreateMap<Defect, DefectResponse>();

        }
    }
}
