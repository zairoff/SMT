using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ProductTransactionDto;
using SMT.ViewModel.Dto.ReadyProductDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class ReadyProductService : IReadyProductService
    {
        private readonly IReadyProductRepository _readyProductRepository;
        private readonly IReadyProductTransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReadyProductService(IUnitOfWork unitOfWork, IReadyProductRepository repository, IMapper mapper, IReadyProductTransactionRepository transactionRepository)
        {
            _unitOfWork = unitOfWork;
            _readyProductRepository = repository;
            _mapper = mapper;
            _transactionRepository = transactionRepository;
        }

        public async Task<IEnumerable<ReadyProductResponse>> GetAllAsync()
        {
            var readyProducts = await _readyProductRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<ReadyProduct>, IEnumerable<ReadyProductResponse>>(readyProducts);
        }

        public async Task<ReadyProductResponse> GetAsync(int id)
        {
            var readyProduct = await _readyProductRepository.FindAsync(x => x.Id == id);

            return _mapper.Map<ReadyProduct, ReadyProductResponse>(readyProduct);
        }

        public async Task<ReadyProductResponse> ImportAsync(ReadyProductCreate readyProductCreate)
        {
            var readyProduct = await _readyProductRepository.FindAsync(p => p.ModelId == readyProductCreate.ModelId);

            if (readyProduct == null)
            {
                readyProduct = _mapper.Map<ReadyProductCreate, ReadyProduct>(readyProductCreate);

                await _readyProductRepository.AddAsync(readyProduct);
            }
            else
            {
                readyProduct.Count += readyProductCreate.Count;
                _readyProductRepository.Update(readyProduct);
            }

            var transaction = new ReadyProductTransaction
            {
                ModelId = readyProductCreate.ModelId,
                Count = readyProductCreate.Count,
                Status = ReadyProductTransactionType.Import,
                Date = DateTime.Now,
            };

            await _transactionRepository.AddAsync(transaction);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReadyProduct, ReadyProductResponse>(readyProduct);
        }

        public async Task<ReadyProductResponse> ExportAsync(ReadyProductUpdate readyProductUpdate)
        {
            var readyProduct = await _readyProductRepository.FindAsync(p => p.ModelId == readyProductUpdate.ModelId);

            if (readyProduct == null)
            {
                throw new NotFoundException("Not found");
            }

            if (readyProduct.Count < readyProductUpdate.Count)
            {
                throw new InvalidOperationException("Not enough");
            }

            readyProduct.Count -= readyProductUpdate.Count;
            _readyProductRepository.Update(readyProduct);

            var transaction = new ReadyProductTransaction
            {
                ModelId = readyProductUpdate.ModelId,
                Count = readyProductUpdate.Count,
                Status = ReadyProductTransactionType.Export,
                Date = DateTime.Now,
            };

            await _transactionRepository.AddAsync(transaction);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReadyProduct, ReadyProductResponse>(readyProduct);
        }

        public async Task<IEnumerable<ReadyProductTransactionResponse>> GetByDateAsync(DateTime date, TransactionType transactionType)
        {
            var productTransactionType = (ReadyProductTransactionType)transactionType;

            IEnumerable<ReadyProductTransaction> readyProducts;

            if (productTransactionType == ReadyProductTransactionType.All)
            {
                readyProducts = await _transactionRepository.GetByAsync(x => x.Date.Date == date.Date);
            }
            else
            {
                readyProducts = await _transactionRepository.GetByAsync(x => x.Date.Date == date.Date && x.Status == productTransactionType);
            }

            return _mapper.Map<IEnumerable<ReadyProductTransaction>, IEnumerable<ReadyProductTransactionResponse>>(readyProducts);
        }

        public async Task<IEnumerable<ReadyProductTransactionResponse>> GetByDateRangeAsync(DateTime from, DateTime to, TransactionType transactionType)
        {
            var productTransactionType = (ReadyProductTransactionType)transactionType;

            IEnumerable<ReadyProductTransaction> readyProducts;

            if (productTransactionType == ReadyProductTransactionType.All)
            {
                readyProducts = await _transactionRepository.GetByAsync(x => x.Date.Date >= from.Date && x.Date.Date <= to);
            }
            else
            {
                readyProducts = await _transactionRepository.GetByAsync(x => x.Date.Date >= from.Date && x.Date.Date <= to && x.Status == productTransactionType);
            }

            return _mapper.Map<IEnumerable<ReadyProductTransaction>, IEnumerable<ReadyProductTransactionResponse>>(readyProducts);
        }

        public async Task<ReadyProductTransactionResponse> DeleteTransactionAsync(int id)
        {
            var readyProductTransaction = await _transactionRepository.FindAsync(p => p.Id == id);

            if (readyProductTransaction == null)
                throw new NotFoundException("Transaction not found");

            _transactionRepository.Delete(readyProductTransaction);

            var readyProduct = await _readyProductRepository.FindAsync(x => x.ModelId == readyProductTransaction.ModelId);

            if (readyProductTransaction == null)
                throw new NotFoundException($"Ready product not found");

            readyProduct.Count -= readyProductTransaction.Count;
            _readyProductRepository.Update(readyProduct);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReadyProductTransaction, ReadyProductTransactionResponse>(readyProductTransaction);
        }
    }
}
