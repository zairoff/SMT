using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.RepairAuditDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class RepairAuditService : IRepairAuditService
    {
        private readonly IRepairAuditRepository _repository;
        private readonly IReportRepository _reportRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RepairAuditService(IRepairAuditRepository repository, IReportRepository reportRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _reportRepository = reportRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<RepairAuditResponse> ScanAsync(RepairAuditCreate repairAuditCreate)
        {
            var report = await _reportRepository.FindAsync(r => r.Barcode == repairAuditCreate.Barcode && r.Status == false);

            if (report == null)
                throw new NotFoundException($"{repairAuditCreate.Barcode} is not a known open board");

            var existing = await _repository.FindByBarcodeAsync(repairAuditCreate.Barcode);

            bool reconfirmed;

            if (existing != null)
            {
                existing.Employee = repairAuditCreate.Employee;
                existing.LastConfirmedDate = DateTime.Now;

                _repository.Update(existing);
                await _unitOfWork.SaveAsync();

                existing = await _repository.FindAsync(a => a.Id == existing.Id);
                reconfirmed = true;

                var updatedResponse = _mapper.Map<RepairAudit, RepairAuditResponse>(existing);
                updatedResponse.Reconfirmed = reconfirmed;
                return updatedResponse;
            }

            var repairAudit = new RepairAudit
            {
                Barcode = repairAuditCreate.Barcode,
                ReportId = report.Id,
                Employee = repairAuditCreate.Employee,
                FirstScannedDate = DateTime.Now,
                LastConfirmedDate = DateTime.Now,
            };

            await _repository.AddAsync(repairAudit);
            await _unitOfWork.SaveAsync();

            repairAudit = await _repository.FindAsync(a => a.Id == repairAudit.Id);

            var response = _mapper.Map<RepairAudit, RepairAuditResponse>(repairAudit);
            response.Reconfirmed = false;

            return response;
        }

        public async Task<RepairAuditResponse> DeleteAsync(int id)
        {
            var repairAudit = await _repository.FindAsync(a => a.Id == id);

            if (repairAudit == null)
                throw new NotFoundException("Not found");

            _repository.Delete(repairAudit);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<RepairAudit, RepairAuditResponse>(repairAudit);
        }

        public async Task RemoveByBarcodeAsync(string barcode)
        {
            var repairAudit = await _repository.FindByBarcodeAsync(barcode);

            if (repairAudit == null)
                return;

            _repository.Delete(repairAudit);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<RepairAuditResponse>> GetAllAsync()
        {
            var repairAudits = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<RepairAudit>, IEnumerable<RepairAuditResponse>>(repairAudits);
        }

        public async Task<RepairAuditResponse> GetAsync(int id)
        {
            var repairAudit = await _repository.FindAsync(a => a.Id == id);

            return _mapper.Map<RepairAudit, RepairAuditResponse>(repairAudit);
        }

        public async Task<IEnumerable<RepairAuditResponse>> GetByDateRangeAsync(DateTime from, DateTime to, int? modelId, int? lineId)
        {
            var repairAudits = await _repository.GetByAsync(a => a.LastConfirmedDate.Date >= from.Date &&
                                                a.LastConfirmedDate.Date <= to.Date &&
                                                (!modelId.HasValue || modelId.Value == 0 || a.Report.ModelId == modelId) &&
                                                (!lineId.HasValue || lineId.Value == 0 || a.Report.LineId == lineId));

            return _mapper.Map<IEnumerable<RepairAudit>, IEnumerable<RepairAuditResponse>>(repairAudits);
        }
    }
}
