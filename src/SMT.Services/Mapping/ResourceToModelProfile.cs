using AutoMapper;
using SMT.ViewModel.Dto.BrandDto;
using SMT.ViewModel.Dto.DefectDto;
using SMT.ViewModel.Dto.ModelDto;
using SMT.ViewModel.Dto.PcbPositionDto;
using SMT.ViewModel.Dto.PcbReportDto;
using SMT.ViewModel.Dto.ProductBrandDto;
using SMT.ViewModel.Dto.ProductDto;
using SMT.Domain;
using Microsoft.EntityFrameworkCore;
using SMT.ViewModel.Dto.DepartmentDto;
using SMT.ViewModel.Dto.LineDto;
using SMT.ViewModel.Dto.LineDefectDto;
using SMT.ViewModel.Dto.ReportDto;
using SMT.ViewModel.Dto.EmployeeDto;
using SMT.ViewModel.Dto.RepairerDto;
using SMT.ViewModel.Dto.MachineDto;
using SMT.ViewModel.Dto.MachineRepairDto;
using SMT.ViewModel.Dto.MachineRepairerDto;
using System;

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
            CreateMap<DepartmentUpdate, Department>();
            CreateMap<DepartmentCreate, Department>()
                .ForMember(o => o.HierarchyId, s => s.MapFrom(o => o.DepartmentId));

            CreateMap<LineCreate, Line>();
            CreateMap<LineUpdate, Line>();

            CreateMap<LineDefectUpdate, LineDefect>();
            CreateMap<LineDefectCreate, LineDefect>();
            
            CreateMap<ReportUpdate, Report>();
            CreateMap<ReportCreate, Report>();

            CreateMap<EmployeeUpdate, Employee>();
            CreateMap<EmployeeCreate, Employee>();

            CreateMap<RepairerCreate, PcbRepairer>();

            CreateMap<MachineCreate, Machine>();

            CreateMap<MachineRepairCreate, MachineRepair>()
                .ForMember(m => m.NotificationDate, s => s.MapFrom(s => string.IsNullOrEmpty(s.NotificationDate) ? (DateTime?)null : DateTime.Parse(s.NotificationDate)))
                .ForMember(m => m.Date, s => s.MapFrom(s => DateTime.Parse(s.CreatedDate)));

            CreateMap<MachineRepairerCreate, MachineRepairer>();
        }
    }
}
