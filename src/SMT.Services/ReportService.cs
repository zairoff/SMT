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

namespace SMT.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportService(IReportRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReportResponse> AddAsync(ReportCreate reportCreate)
        {
            var report = await _repository.FindAsync(p => p.Barcode == reportCreate.Barcode && p.Status == false);

            if (report != null)
                throw new ConflictException($"{reportCreate.Barcode} alredy exists, close the first threat");

            report = _mapper.Map<ReportCreate, Report>(reportCreate);

            await _repository.AddAsync(report);
            await _unitOfWork.SaveAsync();

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

        public async Task<ReportResponse> GetByBarcodeAsync(string barcode)
        {
            var report = await _repository.FindAsync(p => p.Barcode == barcode && p.Status == false);

            return _mapper.Map<Report, ReportResponse>(report);
        }

        public async Task<IEnumerable<ReportResponse>> GetByModelAndLineIdAsync(int modelId, int lineId, DateTime date)
        {
            var reports = await _repository.GetByAsync(r => r.ModelId == modelId && r.LineId == lineId && r.Date.Date == date.Date);

            return _mapper.Map<IEnumerable<Report>, IEnumerable<ReportResponse>>(reports);
        }

        public async Task<ReportResponse> UpdateAsync(int id, ReportUpdate reportUpdate)
        {
            var report = await _repository.FindAsync(p => p.Id == id);

            if (report == null)
                throw new NotFoundException("Report not found");

            _repository.Update(report);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Report, ReportResponse>(report);
        }
    }
}
