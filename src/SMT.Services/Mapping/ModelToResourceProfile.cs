using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.ViewModel.Dto.DefectDto;
using SMT.ViewModel.Dto.DepartmentDto;
using SMT.ViewModel.Dto.ModelDto;
using SMT.Domain;
using SMT.ViewModel.Dto.LineDto;
using SMT.ViewModel.Dto.LineDefectDto;
using SMT.ViewModel.Dto.ReportDto;
using SMT.ViewModel.Dto.EmployeeDto;
using SMT.ViewModel.Dto.RepairerDto;
using SMT.ViewModel.Dto.MachineDto;
using SMT.ViewModel.Dto.MachineRepairDto;
using SMT.ViewModel.Dto.MachineRepairerDto;
using SMT.ViewModel.Dto.PlanDto;
using SMT.ViewModel.Dto.PlanActivityDto;
using SMT.ViewModel.Dto.LineOwnerDto;

namespace SMT.Services.Mapping
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Model, ModelResponse>();

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

            CreateMap<Machine, MachineResponse>();

            CreateMap<MachineRepair, MachineRepairResponse>()
                .ForMember(m => m.CreatedDate, s => s.MapFrom(s => s.Date.ToString("yyyy-MM-dd HH:mm")));

            CreateMap<MachineRepairer, MachineRepairerResponse>();
            CreateMap<LineOwner, LineOwnerResponse>();

            CreateMap<Plan, PlanResponse>()
                .ForMember(m => m.Date, s => s.MapFrom(s => s.Date.ToString("yyyy-MM-dd")));

            CreateMap<PlanActivity, PlanActivityResponse>()
                .ForMember(m => m.Date, s => s.MapFrom(s => s.Date.ToString("yyyy-MM-dd")))
                .ForMember(m => m.Expires, s => s.MapFrom(s => s.Expires.ToString("yyyy-MM-dd")));
        }
    }
}
