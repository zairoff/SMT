using AutoMapper;
using SMT.Common.Dto.PcbReportDto;
using SMT.Common.Dto.BrandDto;
using SMT.Common.Dto.ProductDto;
using SMT.Common.Dto.ModelDto;
using SMT.Domain;
using SMT.Common.Dto.PcbPositionDto;
using SMT.Common.Dto.ProductBrandDto;
using SMT.Common.Dto.DefectDto;
using SMT.Common.Dto.UserDto;
using SMT.Common.Dto.DepartmentDto;
using Microsoft.EntityFrameworkCore;

namespace SMT.Common.Mapping
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
            CreateMap<Department, DepartmentResponse>();
            CreateMap<HierarchyId, string>().ConvertUsing(s => s.ToString());
        }
    }
}
