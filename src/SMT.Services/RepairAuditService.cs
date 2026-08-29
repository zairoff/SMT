using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.RepairAuditDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class RepairAuditService : IRepairAuditService
    {
        private readonly IRepairAuditRepository _repository;
        private readonly IModelRepository _modelRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RepairAuditService(IRepairAuditRepository repository, IModelRepository modelRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _modelRepository = modelRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        // Boards aren't individually registered anywhere - the only thing we can validate a
        // scanned barcode against is that it starts with a known, active model's barcode
        // prefix (the same convention the /report page uses client-side for its own barcode
        // format check). This intentionally does not require the board to already have a
        // defect Report - the audit counts boards that exist, not boards already flagged broken.
        private async Task<Model> FindModelByBarcodeAsync(string barcode)
        {
            var trimmed = barcode?.Trim().ToUpperInvariant() ?? string.Empty;

            var models = await _modelRepository.GetByAsync(m => m.IsActive && !string.IsNullOrEmpty(m.Barcode));

            return models
                .Where(m => trimmed.StartsWith(m.Barcode.Trim().ToUpperInvariant()))
                .OrderByDescending(m => m.Barcode.Length)
                .FirstOrDefault();
        }

        public async Task<RepairAuditResponse> ScanAsync(RepairAuditCreate repairAuditCreate)
        {
            var model = await FindModelByBarcodeAsync(repairAuditCreate.Barcode);

            if (model == null)
                throw new NotFoundException($"{repairAuditCreate.Barcode} does not match any known board model");

            var existing = await _repository.FindByBarcodeAsync(repairAuditCreate.Barcode);

            if (existing != null)
            {
                existing.Employee = repairAuditCreate.Employee;
                existing.LastConfirmedDate = DateTime.Now;

                _repository.Update(existing);
                await _unitOfWork.SaveAsync();

                existing = await _repository.FindAsync(a => a.Id == existing.Id);

                var updatedResponse = _mapper.Map<RepairAudit, RepairAuditResponse>(existing);
                updatedResponse.Reconfirmed = true;
                return updatedResponse;
            }

            var repairAudit = new RepairAudit
            {
                Barcode = repairAuditCreate.Barcode,
                ModelId = model.Id,
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

        public async Task<IEnumerable<RepairAuditResponse>> GetByDateRangeAsync(DateTime from, DateTime to, int? modelId)
        {
            var repairAudits = await _repository.GetByAsync(a => a.LastConfirmedDate.Date >= from.Date &&
                                                a.LastConfirmedDate.Date <= to.Date &&
                                                (!modelId.HasValue || modelId.Value == 0 || a.ModelId == modelId));

            return _mapper.Map<IEnumerable<RepairAudit>, IEnumerable<RepairAuditResponse>>(repairAudits);
        }
    }
}
