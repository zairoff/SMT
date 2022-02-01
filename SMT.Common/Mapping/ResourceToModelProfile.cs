﻿using AutoMapper;
using SMT.Common.Dto.PcbReportDto;
using SMT.Common.Dto.ProductDto;
using SMT.Common.Dto.BrandDto;
using SMT.Common.Dto.ModelDto;
using SMT.Domain;
using SMT.Common.Dto.PcbPositionDto;
using SMT.Common.Dto.ProductBrandDto;
using SMT.Common.Dto.DefectDto;
using SMT.Common.Dto.DepartmentDto;
using Microsoft.EntityFrameworkCore;

namespace SMT.Common.Mapping
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

            CreateMap<string, HierarchyId>().ConvertUsing(s => HierarchyId.Parse(s));
            CreateMap<DepartmentCreate, Department>();                
            CreateMap<DepartmentUpdate, Department>();
        }
    }
}