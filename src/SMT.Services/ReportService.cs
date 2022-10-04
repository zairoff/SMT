using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ReportDto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using SMT.Services.Exceptions;
using SMT.Services.Utils;
using System.Linq;
using SMT.Notification;

namespace SMT.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public ReportService(IReportRepository repository, IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<ReportResponse> AddAsync(ReportCreate reportCreate)
        {
            var report = await _repository.FindAsync(p => p.Barcode == reportCreate.Barcode && p.Status == false);

            if (report != null)
                throw new ConflictException($"{reportCreate.Barcode} alredy exists, close the first threat");

            report = _mapper.Map<ReportCreate, Report>(reportCreate);
            report.CreatedDate = DateTime.Now;

            await _repository.AddAsync(report);
            await _unitOfWork.SaveAsync();

            var reports = await _repository.GetByAsync(r => r.ModelId == reportCreate.ModelId &&
                                            r.LineId == reportCreate.LineId &&
                                            r.DefectId == reportCreate.DefectId &&
                                            r.CreatedDate.Date == DateTime.Now.Date);

            /*var count = await _repository.CountAsync(r => r.ModelId == reportCreate.ModelId &&
                                            r.PositionId == reportCreate.PcbPositionId &&
                                            r.Date.Date == DateTime.Now.Date);*/
            var count = reports.Count();
            if (count > 3)
                await _notificationService.NotifyAsync(reports.ToList());

            return _mapper.Map<Report, ReportResponse>(report);
        }

        public async Task<ReportResponse> DeleteAsync(int id)
        {
            var report = await _repository.FindAsync(p => p.Id == id);

            if (report == null)
                throw new NotFoundException("Report not found");

            _repository.Delete(report);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Report, ReportResponse>(report);
        }

        public async Task<IEnumerable<ReportResponse>> GetAllAsync()
        {
            var reports = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<Report>, IEnumerable<ReportResponse>>(reports);
        }

        public async Task<ReportResponse> GetAsync(int id)
        {
            var report = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<Report, ReportResponse>(report);
        }

        public async Task<IEnumerable<ReportResponse>> GetByAsync(int? productId,
                                                                int? brandId,
                                                                int? modelId,
                                                                int? lineId,
                                                                DateTime from,
                                                                DateTime to)
        {
            var predicate = PredicateBuilder.True<Report>();

            if (productId.HasValue && productId.Value > 0)
            {
                predicate = predicate.And(p => p.Model.ProductBrand.ProductId == productId);
            }

            if (brandId.HasValue && brandId.Value > 0)
            {
                predicate = predicate.And(p => p.Model.ProductBrand.BrandId == brandId);
            }

            if (modelId.HasValue && modelId.Value > 0)
            {
                predicate = predicate.And(p => p.ModelId == modelId);
            }

            if (lineId.HasValue && lineId.Value > 0)
            {
                predicate = predicate.And(p => p.LineId == lineId);
            }

            predicate = predicate.And(p => p.CreatedDate.Date >= from);

            predicate = predicate.And(p => p.CreatedDate.Date <= to);


            var reports = await _repository.GetByAsync(predicate);

            return _mapper.Map<IEnumerable<Report>, IEnumerable<ReportResponse>>(reports);
        }

        public async Task<ReportResponse> GetByBarcodeAsync(string barcode)
        {
            var report = await _repository.FindAsync(p => p.Barcode.Equals(barcode) && p.Status == false);

            return _mapper.Map<Report, ReportResponse>(report);
        }

        public async Task<IEnumerable<ReportResponse>> GetByDateAsync(DateTime date, bool status)
        {
            var reports = await _repository.GetByAsync(p => p.CreatedDate.Date == (date.Date) && p.Status == status);

            return _mapper.Map<IEnumerable<Report>, IEnumerable<ReportResponse>>(reports);
        }

        public async Task<IEnumerable<ReportResponse>> GetByModelAndLineIdAsync(int modelId, int lineId, DateTime date)
        {
            var reports = await _repository.GetByAsync(p => p.CreatedDate.Date == date.Date && p.ModelId == modelId && p.LineId == lineId);

            return _mapper.Map<IEnumerable<Report>, IEnumerable<ReportResponse>>(reports);
        }

        public async Task<ReportResponse> UpdateAsync(int id, ReportUpdate reportUpdate)
        {
            var report = await _repository.FindAsync(p => p.Id == id);

            if (report == null)
                throw new NotFoundException("Report not found");

            report.Action = reportUpdate.Action;
            report.Condition = reportUpdate.Condition;
            report.Employee = reportUpdate.Employee;
            report.Status = reportUpdate.Status;
            report.UpdatedDate = DateTime.Now;

            _repository.Update(report);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Report, ReportResponse>(report);
        }
    }
}
