using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.Access;
using SMT.Common.Dto;
using SMT.Common.Exceptions;
using SMT.Domain;
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
        private readonly IMapper _mapper;

        public PcbReportService(IRepository<PcbReport> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PcbReportResponse> AddAsync(PcbReportCreate reportCreate)
        {
            var report = _mapper.Map<PcbReportCreate, PcbReport>(reportCreate);

            await _repository.AddAsync(report);

            return _mapper.Map<PcbReport, PcbReportResponse>(report);
        }

        public async Task<PcbReportResponse> DeleteAsync(int id)
        {
            var report = await _repository.Get().Where(r => r.Id == id).FirstOrDefaultAsync();

            if (report == null)
                throw new NotFoundException();

            await _repository.DeleteAsync(report);

            return _mapper.Map<PcbReport, PcbReportResponse>(report);
        }

        public async Task<IEnumerable<PcbReportResponse>> GetAllAsync()
        {
            var reports = await _repository.GetAll()
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.Product)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.Brand)
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
                                    .ThenInclude(r => r.Product)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.Brand)
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
                                    .ThenInclude(r => r.Product)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.Brand)
                                    .Include(r => r.Defect)
                                    .Include(r => r.PcbPosition)
                                    .ToListAsync();

            return _mapper.Map<IEnumerable<PcbReport>, IEnumerable<PcbReportResponse>>(reports);
        }

        public async Task<IEnumerable<PcbReportResponse>> GetByModelIdAsync(int modelId)
        {
            var reports = await _repository.GetAll()
                                    .Where(r => r.ModelId == modelId)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.Product)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.Brand)
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
                                    .ThenInclude(r => r.Product)
                                    .Include(r => r.Model)
                                    .ThenInclude(r => r.Brand)
                                    .Include(r => r.Defect)
                                    .Include(r => r.PcbPosition)
                                    .ToListAsync();

            return _mapper.Map<IEnumerable<PcbReport>, IEnumerable<PcbReportResponse>>(reports);
        }

        public async Task<PcbReportResponse> UpdateAsync(int id, PcbReportUpdate report)
        {
            var existingReport = await _repository.Get().Where(r => r.Id == id).FirstOrDefaultAsync();

            if (existingReport == null)
                throw new NotFoundException();

            existingReport.DefectId = report.DefectId;
            existingReport.PcbPositionId = report.PcbPositionId;

            await _repository.UpdateAsync(existingReport);

            return _mapper.Map<PcbReport, PcbReportResponse>(existingReport);
        }
    }
}
