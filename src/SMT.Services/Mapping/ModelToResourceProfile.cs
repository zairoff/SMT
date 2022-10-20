using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.ViewModel.Dto.BrandDto;
using SMT.ViewModel.Dto.DefectDto;
using SMT.ViewModel.Dto.DepartmentDto;
using SMT.ViewModel.Dto.ModelDto;
using SMT.ViewModel.Dto.PcbPositionDto;
using SMT.ViewModel.Dto.PcbReportDto;
using SMT.ViewModel.Dto.ProductBrandDto;
using SMT.ViewModel.Dto.ProductDto;
using SMT.Domain;
using SMT.ViewModel.Dto.UserDto;
using SMT.ViewModel.Dto.LineDto;
using SMT.ViewModel.Dto.LineDefectDto;
using SMT.ViewModel.Dto.ReportDto;
using SMT.ViewModel.Dto.EmployeeDto;
using SMT.ViewModel.Dto.RepairerDto;
using SMT.ViewModel.Dto.MachineDto;
using SMT.ViewModel.Dto.MachineRepairDto;
using SMT.ViewModel.Dto.MachineRepairerDto;
using SMT.ViewModel.Dto.PlanDto;

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
            CreateMap<ProductBrand, ProductBrandResponse>();

            CreateMap<Defect, DefectResponse>();
            //CreateMap<User, UserResponse>();
            CreateMap<HierarchyId, string>().ConvertUsing(s => s.ToString());

            CreateMap<Department, DepartmentResponse>()
                .ForMember(s => s.DepartmentId, o => o.MapFrom(s => s.HierarchyId));

            CreateMap<Line, LineResponse>();
            CreateMap<LineDefect, LineDefectResponse>();

            CreateMap<Report, ReportResponse>();

            CreateMap<Employee, EmployeeResponse>()
                .ForMember(e => e.ImageUrl, o => o.MapFrom(s => s.ImagePath));

            CreateMap<PcbRepairer, RepairerResponse>();

            CreateMap<Machine, MachineResponse>();

            CreateMap<MachineRepair, MachineRepairResponse>()
                .ForMember(m => m.CreatedDate, s => s.MapFrom(s => s.Date.ToString("yyyy-MM-dd HH:mm")))
                .ForMember(m => m.NotificationDate, s => s.MapFrom(s => s.NotificationDate.HasValue ?
                                                s.NotificationDate.Value.ToString("yyyy-MM-dd HH:mm") : ""));

            CreateMap<MachineRepairer, MachineRepairerResponse>();

            CreateMap<Plan, PlanResponse>();
        }
    }
}
