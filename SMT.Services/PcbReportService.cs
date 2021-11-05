using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.Access;
using SMT.Common.Dto.PcbReportDto;
using SMT.Common.Exceptions;
using SMT.Domain;
using SMT.Notification;
using SMT.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class PcbReportService : IPcbReportService
    {
        private readonly IRepository<PcbReport> _repository;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public PcbReportService(IRepository<PcbReport> repository, IMapper mapper, INotificationService notificationService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _mapper = mapper;
        }

        public async Task<PcbReportResponse> AddAsync(PcbReportCreate reportCreate)
        {
            var report = _mapper.Map<PcbReportCreate, PcbReport>(reportCreate);

            await _repository.AddAsync(report);

            var reports = await _repository.GetAll()
                                            .Where(r => r.ModelId == reportCreate.ModelId && 
                                            r.PcbPositionId == reportCreate.PcbPositionId &&
                                            r.Date.Date == DateTime.Now.Date)
                                            .Include(r => r.Model)
                                            .ThenInclude(r => r.ProductBrand)
                                            .ThenInclude(r => r.Product)
                                            .Include(r => r.Model)
                                            .ThenInclude(r => r.ProductBrand)
                                            .ThenInclude(r => r.Brand)
                                            .Include(r => r.Model)
                                            .Include(r => r.Defect)
                                            .Include(r => r.PcbPosition)
                                            .ToListAsync();

            var count = reports.Count;

            if (count > 3)
                await _notificationService.Notify(reports, count);

            return _mapper.Map<PcbReport, PcbReportResponse>(report);
        }

        public async Task<PcbReportResponse> DeleteAsync(int id)
        {
            var report = await _repository.Get().Where(r => r.Id == id)
                                            .Include(r => r.Model)
                                            .ThenInclude(r => r.ProductBrand)
                                            .ThenInclude(r => r.Product)
                                            .Include(r => r.Model)
                                            .ThenInclude(r => r.ProductBrand)
                                            .ThenInclude(r => r.Brand)
                                            .Include(r => r.Model)
                                            .Include(r => r.Defect)
                                            .Include(r => r.PcbPosition)
                                            .FirstOrDefaultAsync();

            if (report == null)
                throw new NotFoundException();

            await _repository.DeleteAsync(report);

            return _mapper.Map<PcbReport, PcbReportResponse>(report);
        }

        public async Task<IEnumerable<PcbReportResponse>> GetAllAsync()
        {
            var reports = await _repository.GetAll()
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Product)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Brand)
                                    .Include(r => r.Model)
                                    .Include(r => r.Defect)
                                    .Include(r => r.PcbPosition)
                                    .ToListAsync();

            return _mapper.Map<IEnumerable<PcbReport>, IEnumerable<PcbReportResponse>>(reports);
        }

        public async Task<PcbReportResponse> GetAsync(int id)
        {
            var report = await _repository.Get()
                                    .Where(r => r.Id == id)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Product)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Brand)
                                    .Include(r => r.Model)
                                    .Include(r => r.Defect)
                                    .Include(r => r.PcbPosition)
                                    .FirstOrDefaultAsync();

            return _mapper.Map<PcbReport, PcbReportResponse>(report);

        }

        public async Task<IEnumerable<PcbReportResponse>> GetByDefectIdAsync(int defectId)
        {
            var reports = await _repository.GetAll()
                                    .Where(r => r.DefectId == defectId)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Product)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Brand)
                                    .Include(r => r.Model)
                                    .Include(r => r.Defect)
                                    .Include(r => r.PcbPosition)
                                    .ToListAsync();

            return _mapper.Map<IEnumerable<PcbReport>, IEnumerable<PcbReportResponse>>(reports);
        }

        public async Task<IEnumerable<PcbReportResponse>> GetByModelIdAsync(int modelId)
        {
            var reports = await _repository.GetAll()
                                    .Where(r => r.ModelId == modelId && r.Date.Date == DateTime.Now.Date)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Product)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Brand)
                                    .Include(r => r.Model)
                                    .Include(r => r.Defect)
                                    .Include(r => r.PcbPosition)
                                    .ToListAsync();

            return _mapper.Map<IEnumerable<PcbReport>, IEnumerable<PcbReportResponse>>(reports);
        }

        public async Task<IEnumerable<PcbReportResponse>> GetByDateAndModelIdAsync(int modelId, DateTime date)
        {
            var reports = await _repository.GetAll()
                                    .Where(r => r.ModelId == modelId && r.Date.Date == date.Date)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Product)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Brand)
                                    .Include(r => r.Model)
                                    .Include(r => r.Defect)
                                    .Include(r => r.PcbPosition)
                                    .ToListAsync();

            return _mapper.Map<IEnumerable<PcbReport>, IEnumerable<PcbReportResponse>>(reports);
        }

        public async Task<IEnumerable<PcbReportResponse>> GetByPositionIdAsync(int positionId)
        {
            var reports = await _repository.GetAll()
                                    .Where(r => r.PcbPositionId == positionId)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Product)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.ProductBrand)
                                    .ThenInclude(r => r.Brand)
                                    .Include(r => r.Model)
                                    .Include(r => r.Defect)
                                    .Include(r => r.PcbPosition)
                                    .ToListAsync();

            return _mapper.Map<IEnumerable<PcbReport>, IEnumerable<PcbReportResponse>>(reports);
        }

        public async Task<PcbReportResponse> UpdateAsync(int id, PcbReportUpdate report)
        {
            var existingReport = await _repository.Get().Where(r => r.Id == id)
                                                    .Include(r => r.Model)
                                                    .ThenInclude(r => r.ProductBrand)
                                                    .ThenInclude(r => r.Product)
                                                    .Include(r => r.Model)
                                                    .ThenInclude(r => r.ProductBrand)
                                                    .ThenInclude(r => r.Brand)
                                                    .Include(r => r.Model)
                                                    .Include(r => r.Defect)
                                                    .Include(r => r.PcbPosition)
                                                    .FirstOrDefaultAsync();

            if (existingReport == null)
                throw new NotFoundException();

            existingReport.DefectId = report.DefectId;
            existingReport.PcbPositionId = report.PcbPositionId;

            await _repository.UpdateAsync(existingReport);

            return _mapper.Map<PcbReport, PcbReportResponse>(existingReport);
        }
    }
}
