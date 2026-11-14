using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.Access.Repository.Interfaces.ReturnedProducts;
using SMT.Access.Unit;
using SMT.Domain.ReturnedProducts;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces.ReturnedProducts;
using SMT.ViewModel.Dto.ReturnedProductScanDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services.ReturnedProducts
{
    public class ReturnedProductScanService : IReturnedProductScanService
    {
        // Only exports leaving repair use the scan-then-batch-export flow.
        private static readonly ReturnedProductTransactionType[] ScanListTransactionTypes =
        {
            ReturnedProductTransactionType.ExportFromRepairToStore,
            ReturnedProductTransactionType.ExportFromRepairToUtilize,
        };

        private readonly IReturnedProductScanRepository _returnedProductScanRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReturnedProductScanService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IReturnedProductScanRepository returnedProductScanRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _returnedProductScanRepository = returnedProductScanRepository;
        }

        public async Task<IEnumerable<ReturnedProductScanResponse>> GetByTransactionTypeAsync(ReturnedProductTransactionType transactionType)
        {
            EnsureScanListTransactionType(transactionType);

            var scans = await _returnedProductScanRepository.GetByAsync(x => x.TransactionType == transactionType);

            return _mapper.Map<IEnumerable<ReturnedProductScan>, IEnumerable<ReturnedProductScanResponse>>(scans);
        }

        public async Task<ReturnedProductScanResponse> AddAsync(ReturnedProductScanCreate returnedProductScanCreate)
        {
            EnsureScanListTransactionType(returnedProductScanCreate.TransactionType);

            var scan = _mapper.Map<ReturnedProductScanCreate, ReturnedProductScan>(returnedProductScanCreate);
            scan.Date = DateTime.Now;

            await _returnedProductScanRepository.AddAsync(scan);
            await _unitOfWork.SaveAsync();

            var created = await _returnedProductScanRepository.FindAsync(x => x.Id == scan.Id);

            return _mapper.Map<ReturnedProductScan, ReturnedProductScanResponse>(created);
        }

        public async Task<ReturnedProductScanResponse> DeleteAsync(int id)
        {
            var scan = await _returnedProductScanRepository.FindAsync(x => x.Id == id);

            if (scan == null)
            {
                throw new NotFoundException("Scan not found");
            }

            _returnedProductScanRepository.Delete(scan);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReturnedProductScan, ReturnedProductScanResponse>(scan);
        }

        // Removes the given scans and returns the ones actually removed, so that
        // when two devices export at the same time each scan is exported only once.
        public async Task<IEnumerable<ReturnedProductScanResponse>> ClaimAsync(ReturnedProductScanClaim returnedProductScanClaim)
        {
            EnsureScanListTransactionType(returnedProductScanClaim.TransactionType);

            var ids = (returnedProductScanClaim.Ids ?? Enumerable.Empty<int>()).Distinct().ToList();
            var transactionType = returnedProductScanClaim.TransactionType;

            var scans = (await _returnedProductScanRepository.GetByAsync(x => x.TransactionType == transactionType && ids.Contains(x.Id))).ToList();

            foreach (var scan in scans)
            {
                _returnedProductScanRepository.Delete(scan);
            }

            try
            {
                await _unitOfWork.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConflictException("Scans are already being exported from another device");
            }

            return _mapper.Map<IEnumerable<ReturnedProductScan>, IEnumerable<ReturnedProductScanResponse>>(scans);
        }

        private static void EnsureScanListTransactionType(ReturnedProductTransactionType transactionType)
        {
            if (!ScanListTransactionTypes.Contains(transactionType))
            {
                throw new InvalidOperationException("Transaction type does not use a scan list");
            }
        }
    }
}
