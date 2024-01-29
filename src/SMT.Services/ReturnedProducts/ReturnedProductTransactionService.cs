using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Repository.Interfaces.ReturnedProducts;
using SMT.Access.Unit;
using SMT.Domain.ReturnedProducts;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces.ReturnedProducts;
using SMT.ViewModel.Dto.ReturnedProductTransactionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services.ReturnedProducts
{
    public class ReturnedProductTransactionService : IReturnedProductTransactionService
    {
        private readonly IModelRepository _modelRepository;
        private readonly IReturnedProductTransactionRepository _returnedProductTransactionRepository;
        private readonly IReturnedProductRepairRepository _returnedProductRepairRepository;
        private readonly IReturnedProductStoreRepository _returnedProductStoreRepository;
        private readonly IReturnedProductUtilizeRepository _returnedProductUtilizeRepository;
        private readonly IReturnedProductBufferRepository _returnedProductBufferRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReturnedProductTransactionService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IModelRepository modelRepository,
            IReturnedProductTransactionRepository returnedProductTransactionRepository,
            IReturnedProductRepairRepository returnedProductRepairRepository,
            IReturnedProductStoreRepository returnedProductStoreRepository,
            IReturnedProductUtilizeRepository returnedProductUtilizeRepository,
            IReturnedProductBufferRepository returnedProductBufferRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _modelRepository = modelRepository;
            _returnedProductTransactionRepository = returnedProductTransactionRepository;
            _returnedProductRepairRepository = returnedProductRepairRepository;
            _returnedProductStoreRepository = returnedProductStoreRepository;
            _returnedProductUtilizeRepository = returnedProductUtilizeRepository;
            _returnedProductBufferRepository = returnedProductBufferRepository;
        }

        public async Task<ReturnedProductTransactionResponse> DeleteTransactionAsync(int id)
        {
            var transaction = await _returnedProductTransactionRepository.FindAsync(p => p.Id == id);

            if (transaction == null)
            {
                throw new InvalidOperationException("Not found");
            }

            var buffer = await _returnedProductBufferRepository.FindAsync(x => x.ReturnedProductTransactionId == transaction.Id);
            var store = await _returnedProductStoreRepository.FindAsync(x => x.ReturnedProductTransactionId == transaction.Id);
            var repair = await _returnedProductRepairRepository.FindAsync(x => x.ReturnedProductTransactionId == transaction.Id);
            var utilize = await _returnedProductUtilizeRepository.FindAsync(x => x.ReturnedProductTransactionId == transaction.Id);

            if (buffer != null)
            {
                _returnedProductBufferRepository.Delete(buffer);
            }

            if (store != null)
            {
                _returnedProductStoreRepository.Delete(store);
            }

            if (repair != null)
            {
                _returnedProductRepairRepository.Delete(repair);
            }

            if (utilize != null)
            {
                _returnedProductUtilizeRepository.Delete(utilize);
            }

            transaction.TransactionType = ReturnedProductTransactionType.Deleted;

            _returnedProductTransactionRepository.Update(transaction);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReturnedProductTransaction, ReturnedProductTransactionResponse>(transaction);
        }

        public async Task<ReturnedProductTransactionResponse> ImportFromFactoryToBufferAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate)
        {
            var model = await _modelRepository.FindAsync(x => x.Id == returnedProductTransactionCreate.ModelId);

            if (model == null)
            {
                throw new NotFoundException("Model not found");
            }

            try
            {
                var transaction = new ReturnedProductTransaction
                {
                    Barcode = returnedProductTransactionCreate.Barcode,
                    ModelId = returnedProductTransactionCreate.ModelId,
                    Count = returnedProductTransactionCreate.Count,
                    TransactionType = returnedProductTransactionCreate.TransactionType,
                    Date = DateTime.Now,
                };

                await _returnedProductTransactionRepository.AddAsync(transaction);

                await _unitOfWork.SaveAsync();

                var returnedBuffer = _mapper.Map<ReturnedProductTransaction, ReturnedProductBufferZone>(transaction);

                await _returnedProductBufferRepository.AddAsync(returnedBuffer);

                await _unitOfWork.SaveAsync();

                return _mapper.Map<ReturnedProductTransaction, ReturnedProductTransactionResponse>(transaction);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ReturnedProductTransactionResponse> ImportFromRepairToStoreAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate)
        {
            var model = await _modelRepository.FindAsync(x => x.Id == returnedProductTransactionCreate.ModelId);

            if (model == null)
            {
                throw new NotFoundException("Model not found");
            }

            var countInRepair = await _returnedProductRepairRepository.FindSumAsync(x => x.ModelId == returnedProductTransactionCreate.ModelId);

            if (countInRepair < returnedProductTransactionCreate.Count)
            {
                throw new Exception($"Not enough count in repair table. Count: {countInRepair}");
            }

            var transaction = new ReturnedProductTransaction
            {
                Barcode = returnedProductTransactionCreate.Barcode,
                ModelId = returnedProductTransactionCreate.ModelId,
                Count = returnedProductTransactionCreate.Count,
                TransactionType = returnedProductTransactionCreate.TransactionType,
                Date = DateTime.Now,
            };

            await _returnedProductTransactionRepository.AddAsync(transaction);

            await _unitOfWork.SaveAsync();

            var returnedProductRepair = _mapper.Map<ReturnedProductTransaction, ReturnedProductRepair>(transaction);
            returnedProductRepair.Count *= -1;

            var returnedProductStore = _mapper.Map<ReturnedProductTransaction, ReturnedProductStore>(transaction);

            await _returnedProductRepairRepository.AddAsync(returnedProductRepair);
            await _returnedProductStoreRepository.AddAsync(returnedProductStore);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReturnedProductTransaction, ReturnedProductTransactionResponse>(transaction);
        }

        public async Task<ReturnedProductTransactionResponse> ImportFromRepairToUtilizeAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate)
        {
            var model = await _modelRepository.FindAsync(x => x.Id == returnedProductTransactionCreate.ModelId);

            if (model == null)
            {
                throw new NotFoundException("Model not found");
            }

            var countInRepair = await _returnedProductRepairRepository.FindSumAsync(x => x.ModelId == returnedProductTransactionCreate.ModelId);

            if (countInRepair < returnedProductTransactionCreate.Count)
            {
                throw new Exception($"Not enough count in repair table. Count: {countInRepair}");
            }

            var transaction = new ReturnedProductTransaction
            {
                Barcode = returnedProductTransactionCreate.Barcode,
                ModelId = returnedProductTransactionCreate.ModelId,
                Count = returnedProductTransactionCreate.Count,
                TransactionType = returnedProductTransactionCreate.TransactionType,
                Date = DateTime.Now,
            };

            await _returnedProductTransactionRepository.AddAsync(transaction);

            await _unitOfWork.SaveAsync();

            var returnedProductRepair = _mapper.Map<ReturnedProductTransaction, ReturnedProductRepair>(transaction);
            returnedProductRepair.Count *= -1;

            var returnedProductUtilize = _mapper.Map<ReturnedProductTransaction, ReturnedProductUtilize>(transaction);

            await _returnedProductRepairRepository.AddAsync(returnedProductRepair);
            await _returnedProductUtilizeRepository.AddAsync(returnedProductUtilize);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReturnedProductTransaction, ReturnedProductTransactionResponse>(transaction);
        }

        public async Task<ReturnedProductTransactionResponse> ExportFromStoreToUtilizeAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate)
        {
            var model = await _modelRepository.FindAsync(x => x.Id == returnedProductTransactionCreate.ModelId);

            if (model == null)
            {
                throw new NotFoundException("Model not found");
            }

            var countInUtilize = await _returnedProductUtilizeRepository.FindSumAsync(x => x.ModelId == returnedProductTransactionCreate.ModelId);

            if (countInUtilize < returnedProductTransactionCreate.Count)
            {
                throw new Exception($"Not enough count in table. Count: {countInUtilize}");
            }

            var transaction = new ReturnedProductTransaction
            {
                Barcode = returnedProductTransactionCreate.Barcode,
                ModelId = returnedProductTransactionCreate.ModelId,
                Count = returnedProductTransactionCreate.Count,
                TransactionType = returnedProductTransactionCreate.TransactionType,
                Date = DateTime.Now,
            };

            await _returnedProductTransactionRepository.AddAsync(transaction);

            await _unitOfWork.SaveAsync();

            var returnedProductUtilize = _mapper.Map<ReturnedProductTransaction, ReturnedProductUtilize>(transaction);
            returnedProductUtilize.Count *= -1;

            await _returnedProductUtilizeRepository.AddAsync(returnedProductUtilize);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReturnedProductTransaction, ReturnedProductTransactionResponse>(transaction);
        }

        public async Task<ReturnedProductTransactionResponse> ExportFromBufferToRepairAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate)
        {
            var model = await _modelRepository.FindAsync(x => x.Id == returnedProductTransactionCreate.ModelId);

            if (model == null)
            {
                throw new NotFoundException("Model not found");
            }

            var countInBuffer = await _returnedProductBufferRepository.FindSumAsync(x => x.ModelId == returnedProductTransactionCreate.ModelId);

            if (countInBuffer < returnedProductTransactionCreate.Count)
            {
                throw new Exception($"Not enough count in table. Count: {countInBuffer}");
            }

            var transaction = new ReturnedProductTransaction
            {
                Barcode = returnedProductTransactionCreate.Barcode,
                ModelId = returnedProductTransactionCreate.ModelId,
                Count = returnedProductTransactionCreate.Count,
                TransactionType = returnedProductTransactionCreate.TransactionType,
                Date = DateTime.Now,
            };

            await _returnedProductTransactionRepository.AddAsync(transaction);

            await _unitOfWork.SaveAsync();

            var returnedProductBuffer = _mapper.Map<ReturnedProductTransaction, ReturnedProductBufferZone>(transaction);
            returnedProductBuffer.Count *= -1;

            var returnedProductRepair = _mapper.Map<ReturnedProductTransaction, ReturnedProductRepair>(transaction);

            await _returnedProductBufferRepository.AddAsync(returnedProductBuffer);
            await _returnedProductRepairRepository.AddAsync(returnedProductRepair);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReturnedProductTransaction, ReturnedProductTransactionResponse>(transaction);
        }

        public async Task<ReturnedProductTransactionResponse> ExportFromStoreToFactoryAsync(ReturnedProductTransactionCreate returnedProductTransactionCreate)
        {
            var model = await _modelRepository.FindAsync(x => x.Id == returnedProductTransactionCreate.ModelId);

            if (model == null)
            {
                throw new NotFoundException("Model not found");
            }

            var countInStore = await _returnedProductStoreRepository.FindSumAsync(x => x.ModelId == returnedProductTransactionCreate.ModelId);

            if (countInStore < returnedProductTransactionCreate.Count)
            {
                throw new Exception($"Not enough count in table. Count: {countInStore}");
            }

            var transaction = new ReturnedProductTransaction
            {
                Barcode = returnedProductTransactionCreate.Barcode,
                ModelId = returnedProductTransactionCreate.ModelId,
                Count = returnedProductTransactionCreate.Count,
                TransactionType = returnedProductTransactionCreate.TransactionType,
                Date = DateTime.Now,
            };

            await _returnedProductTransactionRepository.AddAsync(transaction);

            await _unitOfWork.SaveAsync();

            var returnedProductStore = _mapper.Map<ReturnedProductTransaction, ReturnedProductStore>(transaction);
            returnedProductStore.Count *= -1;

            await _returnedProductStoreRepository.AddAsync(returnedProductStore);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReturnedProductTransaction, ReturnedProductTransactionResponse>(transaction);
        }

        public async Task<IEnumerable<ReturnedProductTransactionResponse>> GetByDateAsync(DateTime date, ReturnedProductTransactionType transactionType)
        {
            IEnumerable<ReturnedProductTransaction> returnedProductTransactions;

            if (transactionType == ReturnedProductTransactionType.All)
            {
                returnedProductTransactions = await _returnedProductTransactionRepository.GetByAsync(x => x.Date.Date == date.Date);
            }
            else
            {
                returnedProductTransactions = await _returnedProductTransactionRepository.GetByAsync(x => x.Date.Date == date.Date && x.TransactionType == transactionType);
            }

            return _mapper.Map<IEnumerable<ReturnedProductTransaction>, IEnumerable<ReturnedProductTransactionResponse>>(returnedProductTransactions);
        }

        public async Task<IEnumerable<ReturnedProductTransactionResponse>> GetByDateGroupByAsync(DateTime date, ReturnedProductTransactionType transactionType)
        {
            IEnumerable<ReturnedProductTransaction> readyProducts = await _returnedProductTransactionRepository.GetGroupByModelAsync(x => x.Date.Date == date.Date && x.TransactionType == transactionType);

            return _mapper.Map<IEnumerable<ReturnedProductTransaction>, IEnumerable<ReturnedProductTransactionResponse>>(readyProducts);
        }

        public async Task<IEnumerable<ReturnedProductTransactionResponse>> GetByDateRangeAsync(DateTime from, DateTime to, ReturnedProductTransactionType transactionType)
        {
            IEnumerable<ReturnedProductTransaction> returnedProductTransactions;

            if (transactionType == ReturnedProductTransactionType.All)
            {
                returnedProductTransactions = await _returnedProductTransactionRepository.GetByAsync(x => x.Date.Date >= from.Date && x.Date.Date <= to);
            }
            else
            {
                returnedProductTransactions = await _returnedProductTransactionRepository.GetByAsync(x => x.Date.Date >= from.Date && x.Date.Date <= to && x.TransactionType == transactionType);
            }

            return _mapper.Map<IEnumerable<ReturnedProductTransaction>, IEnumerable<ReturnedProductTransactionResponse>>(returnedProductTransactions);
        }

        public async Task<IEnumerable<ReturnedProductTransactionResponse>> GetByDateRangeGroupByAsync(DateTime from, DateTime to, ReturnedProductTransactionType transactionType)
        {
            var readyProducts = await _returnedProductTransactionRepository.GetGroupByModelAsync(x => x.Date.Date >= from.Date && x.Date.Date <= to && x.TransactionType == transactionType);

            return _mapper.Map<IEnumerable<ReturnedProductTransaction>, IEnumerable<ReturnedProductTransactionResponse>>(readyProducts);
        }

        public async Task<IEnumerable<ReturnedProductTransactionResponse>> GetStoreStateAsync()
        {
            var returnedProductsStore = await _returnedProductStoreRepository.GetGroupByModelAsync();

            var filtered = returnedProductsStore.Where(x => x.Count > 0);

            return _mapper.Map<IEnumerable<ReturnedProductStore>, IEnumerable<ReturnedProductTransactionResponse>>(filtered);
        }

        public async Task<IEnumerable<ReturnedProductTransactionResponse>> GetRepairStateAsync()
        {
            var returnedProductsRepair = await _returnedProductRepairRepository.GetGroupByModelAsync();

            var filtered = returnedProductsRepair.Where(x => x.Count > 0);

            return _mapper.Map<IEnumerable<ReturnedProductRepair>, IEnumerable<ReturnedProductTransactionResponse>>(filtered);
        }

        public async Task<IEnumerable<ReturnedProductTransactionResponse>> GetUtilizeStateAsync()
        {
            var returnedProductsUtilize = await _returnedProductUtilizeRepository.GetGroupByModelAsync();

            var filtered = returnedProductsUtilize.Where(x => x.Count > 0);

            return _mapper.Map<IEnumerable<ReturnedProductUtilize>, IEnumerable<ReturnedProductTransactionResponse>>(filtered);
        }

        public async Task<IEnumerable<ReturnedProductTransactionResponse>> GetBufferStateAsync()
        {
            var returnedProductsBuffer = await _returnedProductBufferRepository.GetGroupByModelAsync();

            var filtered = returnedProductsBuffer.Where(x => x.Count > 0);

            return _mapper.Map<IEnumerable<ReturnedProductBufferZone>, IEnumerable<ReturnedProductTransactionResponse>>(filtered);
        }
    }
}
