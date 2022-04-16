using AutoMapper;
using SMT.ViewModel.Dto.BrandDto;
using SMT.ViewModel.Dto.DefectDto;
using SMT.ViewModel.Dto.ModelDto;
using SMT.ViewModel.Dto.PcbPositionDto;
using SMT.ViewModel.Dto.PcbReportDto;
using SMT.ViewModel.Dto.ProductBrandDto;
using SMT.ViewModel.Dto.ProductDto;
using SMT.ViewModel.Dto.UserDto;
using SMT.Domain;
using Microsoft.EntityFrameworkCore;
using SMT.ViewModel.Dto.DepartmentDto;
using SMT.ViewModel.Dto.LineDto;

namespace SMT.Services.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<PcbReportCreate, PcbReport>();
            CreateMap<PcbReportUpdate, PcbReport>();

            CreateMap<ProductCreate, Product>();
            CreateMap<ProductUpdate, Product>();

            CreateMap<BrandCreate, Brand>();
            CreateMap<BrandUpdate, Brand>();

            CreateMap<ModelCreate, Model>();
            CreateMap<ModelUpdate, Model>();

            CreateMap<PcbPositionCreate, PcbPosition>();
            CreateMap<PcbPositionUpdate, PcbPosition>();

            CreateMap<ProductBrandCreate, ProductBrand>();
            CreateMap<ProductBrandUpdate, ProductBrand>();

            CreateMap<DefectCreate, Defect>();
            CreateMap<DefectUpdate, Defect>();

            //CreateMap<UserCreate, User>();
            //CreateMap<UserUpdate, User>();

            CreateMap<string, HierarchyId>().ConvertUsing(s => HierarchyId.Parse(s));
            CreateMap<DepartmentCreate, Department>();
            CreateMap<DepartmentUpdate, Department>();

            CreateMap<LineCreate, Line>();
            CreateMap<LineUpdate, Line>();
        }
    }
}
