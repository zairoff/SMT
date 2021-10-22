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
        }
    }
}
