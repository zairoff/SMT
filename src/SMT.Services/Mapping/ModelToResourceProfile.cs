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
using SMT.ViewModel.Dto.LineDto;
using SMT.ViewModel.Dto.LineDefectDto;
using SMT.ViewModel.Dto.ReportDto;
using SMT.ViewModel.Dto.EmployeeDto;
using SMT.ViewModel.Dto.RepairerDto;
using SMT.ViewModel.Dto.MachineDto;
using SMT.ViewModel.Dto.MachineRepairDto;
using SMT.ViewModel.Dto.MachineRepairerDto;
using SMT.ViewModel.Dto.RepairAuditDto;
using SMT.ViewModel.Dto.PlanDto;
using SMT.ViewModel.Dto.PlanActivityDto;
using SMT.ViewModel.Dto.ReadyProductDto;
using SMT.ViewModel.Dto.ProductTransactionDto;
using System;
using SMT.Domain.ReturnedProducts;
using SMT.ViewModel.Dto.ReturnedProductTransactionDto;
using SMT.ViewModel.Dto.HourlyPlanDto;
using SMT.ViewModel.Dto.ComponentDto;
using SMT.ViewModel.Dto.QrReaderDto;
using SMT.Domain.BoardFlow;
using SMT.ViewModel.Dto.BoardReportDto;
using SMT.Domain.Service;
using SMT.ViewModel.Dto.ServiceCenterDto;
using SMT.ViewModel.Dto.InstructionPositionDto;
using SMT.ViewModel.Dto.LineActiveModelDto;
using SMT.ViewModel.Dto.ModelInstructionImageDto;

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
                .ForMember(e => e.ImageUrl, o => o.MapFrom(s => s.ImagePath))
                .ForMember(e => e.Birthday, o => o.MapFrom(s => s.Birthday.ToString("yyyy-MM-dd")));

            CreateMap<PcbRepairer, RepairerResponse>();

            CreateMap<Machine, MachineResponse>();

            CreateMap<MachineRepair, MachineRepairResponse>()
                .ForMember(m => m.CreatedDate, s => s.MapFrom(s => s.Date.ToString("yyyy-MM-dd HH:mm")))
                .ForMember(m => m.NotificationDate, s => s.MapFrom(s => s.NotificationDate.HasValue ?
                                                s.NotificationDate.Value.ToString("yyyy-MM-dd HH:mm") : ""));

            CreateMap<MachineRepairer, MachineRepairerResponse>();

            CreateMap<RepairAudit, RepairAuditResponse>()
                .ForMember(m => m.ModelName, s => s.MapFrom(s => s.Report.Model.Name))
                .ForMember(m => m.LineName, s => s.MapFrom(s => s.Report.Line.Name))
                .ForMember(m => m.FirstScannedDate, s => s.MapFrom(s => s.FirstScannedDate.ToString("yyyy-MM-dd HH:mm")))
                .ForMember(m => m.LastConfirmedDate, s => s.MapFrom(s => s.LastConfirmedDate.ToString("yyyy-MM-dd HH:mm")))
                .ForMember(m => m.Reconfirmed, s => s.Ignore());

            CreateMap<Plan, PlanResponse>()
                .ForMember(m => m.Date, s => s.MapFrom(s => s.Date.ToString("yyyy-MM-dd")));

            CreateMap<PlanActivity, PlanActivityResponse>()
                .ForMember(m => m.Date, s => s.MapFrom(s => s.Date.ToString("yyyy-MM-dd")))
                .ForMember(m => m.Expires, s => s.MapFrom(s => s.Expires.ToString("yyyy-MM-dd")));

            CreateMap<ReadyProduct, ReadyProductResponse>();

            CreateMap<ReadyProductTransaction, ReadyProductTransactionResponse>()
               .ForMember(m => m.Date, s => s.MapFrom(s => s.Date.ToString("yyyy-MM-dd HH:mm:ss")))
               .ForMember(m => m.Status, s => s.MapFrom(s => s.Status.ToString()))
               .ForMember(m => m.Count, s => s.MapFrom(s => Math.Abs(s.Count)));

            CreateMap<ReturnedProductTransaction, ReturnedProductTransactionResponse>()
               .ForMember(m => m.Date, s => s.MapFrom(s => s.Date.ToString("yyyy-MM-dd HH:mm:ss")))
               .ForMember(m => m.TransactionType, s => s.MapFrom(s => s.TransactionType.ToString()))
               .ForMember(m => m.Count, s => s.MapFrom(s => Math.Abs(s.Count)));

            CreateMap<ReturnedProductStore, ReturnedProductTransactionResponse>()
               .ForMember(m => m.Count, s => s.MapFrom(s => Math.Abs(s.Count)));

            CreateMap<ReturnedProductRepair, ReturnedProductTransactionResponse>()
               .ForMember(m => m.Count, s => s.MapFrom(s => Math.Abs(s.Count)));

            CreateMap<ReturnedProductUtilize, ReturnedProductTransactionResponse>()
               .ForMember(m => m.Count, s => s.MapFrom(s => Math.Abs(s.Count)));

            CreateMap<ReturnedProductBufferZone, ReturnedProductTransactionResponse>()
               .ForMember(m => m.Count, s => s.MapFrom(s => Math.Abs(s.Count)));


            CreateMap<HourlyPlan, HourlyPlanResponse>()
               .ForMember(m => m.Time, s => s.MapFrom(s => s.Time.ToString("yyyy-MM-dd HH:mm:ss")));

            CreateMap<Component, ComponentResponse>();

            CreateMap<QrReader, QrReaderResponse>();
            CreateMap<BoardReport, BoardReportResponse>()
                .ForMember(m => m.DateTime, s => s.MapFrom(s => s.DateTime.ToString("yyyy-MM-dd HH:mm:ss")));

            CreateMap<ServiceCenter, ServiceCenterResponse>();

            CreateMap<InstructionPosition, InstructionPositionResponse>()
                .ForMember(p => p.LineName, o => o.MapFrom(s => s.Line != null ? s.Line.Name : null));

            CreateMap<LineActiveModel, LineActiveModelResponse>()
                .ForMember(a => a.LineName, o => o.MapFrom(s => s.Line != null ? s.Line.Name : null))
                .ForMember(a => a.ModelName, o => o.MapFrom(s => s.Model != null ? s.Model.Name : null));

            CreateMap<ModelInstructionImage, ModelInstructionImageResponse>()
                .ForMember(i => i.ModelName, o => o.MapFrom(s => s.Model != null ? s.Model.Name : null))
                .ForMember(i => i.PositionName, o => o.MapFrom(s => s.InstructionPosition != null ? s.InstructionPosition.Name : null));
        }
    }
}
