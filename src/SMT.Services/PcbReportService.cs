using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.ViewModel.Dto.PcbReportDto;
using SMT.Domain;
using SMT.Notification;
using SMT.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMT.Services.Exceptions;

namespace SMT.Services
{
    public class PcbReportService : IPcbReportService
    {
        private readonly IPcbReportRepository _repository;
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PcbReportService(IPcbReportRepository repository, IMapper mapper, INotificationService notificationService, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _notificationService = notificationService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PcbReportResponse> AddAsync(PcbReportCreate reportCreate)
        {
            var report = _mapper.Map<PcbReportCreate, PcbReport>(reportCreate);

            await _repository.AddAsync(report);
            await _unitOfWork.SaveAsync();

            // TODO: Need to change the structure. Really bad designed
            var reports = await _repository.GetByAsync(r => r.ModelId == reportCreate.ModelId &&
                                            r.PositionId == reportCreate.PcbPositionId &&
                                            r.Date.Date == DateTime.Now.Date);

            var count = await _repository.CountAsync(r => r.ModelId == reportCreate.ModelId &&
                                            r.PositionId == reportCreate.PcbPositionId &&
                                            r.Date.Date == DateTime.Now.Date);

            if (count > 3)
                await _notificationService.Notify(reports.ToList(), count);

            return _mapper.Map<PcbReport, PcbReportResponse>(report);
        }

        public async Task<PcbReportResponse> DeleteAsync(int id)
        {
            var report = await _repository.FindAsync(r => r.Id == id);

            if (report == null)
                throw new NotFoundException("Not found");

            _repository.Delete(report);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<PcbReport, PcbReportResponse>(report);
        }

        public async Task<IEnumerable<PcbReportResponse>> GetAllAsync()
        {
            var reports = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<PcbReport>, IEnumerable<PcbReportResponse>>(reports);
        }

        public async Task<PcbReportResponse> GetAsync(int id)
        {
            var report = await _repository.FindAsync(r => r.Id == id);

            return _mapper.Map<PcbReport, PcbReportResponse>(report);

        }

        public async Task<IEnumerable<PcbReportResponse>> GetByDefectIdAsync(int defectId)
        {
            var reports = await _repository.GetByAsync(r => r.DefectId == defectId);

            return _mapper.Map<IEnumerable<PcbReport>, IEnumerable<PcbReportResponse>>(reports);
        }

        public async Task<IEnumerable<PcbReportResponse>> GetByModelIdAsync(int modelId)
        {
            var reports = await _repository.GetByAsync(r => r.ModelId == modelId && r.Date.Date == DateTime.Now.Date);

            return _mapper.Map<IEnumerable<PcbReport>, IEnumerable<PcbReportResponse>>(reports);
        }

        public async Task<IEnumerable<PcbReportResponse>> GetByDateAndModelIdAsync(int modelId, DateTime date)
        {
            var reports = await _repository.GetByAsync(r => r.ModelId == modelId && r.Date.Date == date.Date);

            return _mapper.Map<IEnumerable<PcbReport>, IEnumerable<PcbReportResponse>>(reports);
        }

        public async Task<IEnumerable<PcbReportResponse>> GetByPositionIdAsync(int positionId)
        {
            var reports = await _repository.GetByAsync(r => r.PositionId == positionId);

            return _mapper.Map<IEnumerable<PcbReport>, IEnumerable<PcbReportResponse>>(reports);
        }

        public async Task<PcbReportResponse> UpdateAsync(int id, PcbReportUpdate report)
        {
            var existingReport = await _repository.FindAsync(r => r.Id == id);

            if (existingReport == null)
                throw new NotFoundException("Not found");

            existingReport.DefectId = report.DefectId;
            existingReport.PositionId = report.PcbPositionId;

            _repository.Update(existingReport);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<PcbReport, PcbReportResponse>(existingReport);
        }
    }
}
