using AutoMapper;
using CoreHtmlToImage;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Repository.Interfaces.ReturnedProducts;
using SMT.Access.Unit;
using SMT.Domain.ReturnedProducts;
using SMT.Notification;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces.ReturnedProducts;
using SMT.ViewModel.Dto.ReturnedProductTransactionDto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        private readonly INotificationService _notificationService;

        public ReturnedProductTransactionService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IModelRepository modelRepository,
            IReturnedProductTransactionRepository returnedProductTransactionRepository,
            IReturnedProductRepairRepository returnedProductRepairRepository,
            IReturnedProductStoreRepository returnedProductStoreRepository,
            IReturnedProductUtilizeRepository returnedProductUtilizeRepository,
            IReturnedProductBufferRepository returnedProductBufferRepository,
            INotificationService notificationService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _modelRepository = modelRepository;
            _returnedProductTransactionRepository = returnedProductTransactionRepository;
            _returnedProductRepairRepository = returnedProductRepairRepository;
            _returnedProductStoreRepository = returnedProductStoreRepository;
            _returnedProductUtilizeRepository = returnedProductUtilizeRepository;
            _returnedProductBufferRepository = returnedProductBufferRepository;
            _notificationService = notificationService;
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

        public async Task GroupByNotifyAsync()
        {
            var from = DateTime.Now.AddDays(-7).Date;
            var to = DateTime.Now.Date;

            // Buffer
            var importFromFactoryToBuffer = await _returnedProductTransactionRepository.GetGroupByModelAsync(
                x => x.TransactionType == ReturnedProductTransactionType.ImportFromFactoryToBuffer && x.Date.Date >= from && x.Date.Date <= to);

            var exportFromBufferToRepair = await _returnedProductTransactionRepository.GetGroupByModelAsync(
                x => x.TransactionType == ReturnedProductTransactionType.ExportFromBufferToRepair && x.Date.Date >= from && x.Date.Date <= to);

            var bufferState = await _returnedProductBufferRepository.GetGroupByModelAsync();
            bufferState = bufferState.Where(x => x.Count > 0);

            // Repair
            var exportFromRepairToStore = await _returnedProductTransactionRepository.GetGroupByModelAsync(
               x => x.TransactionType == ReturnedProductTransactionType.ExportFromRepairToStore && x.Date.Date >= from && x.Date.Date <= to);

            var exportFromRepairToUtilize = await _returnedProductTransactionRepository.GetGroupByModelAsync(
               x => x.TransactionType == ReturnedProductTransactionType.ExportFromRepairToUtilize && x.Date.Date >= from && x.Date.Date <= to);

            var reapirState = await _returnedProductRepairRepository.GetGroupByModelAsync();
            reapirState = reapirState.Where(x => x.Count > 0);

            // Store
            var exportFromStoreToFactory = await _returnedProductTransactionRepository.GetGroupByModelAsync(
               x => x.TransactionType == ReturnedProductTransactionType.ExportFromStoreToFactory && x.Date.Date >= from && x.Date.Date <= to);

            var exportFromStoreToUtilize = await _returnedProductTransactionRepository.GetGroupByModelAsync(
               x => x.TransactionType == ReturnedProductTransactionType.ExportFromStoreToUtilize && x.Date.Date >= from && x.Date.Date <= to);

            var storeState = await _returnedProductStoreRepository.GetGroupByModelAsync();
            storeState = storeState.Where(x => x.Count > 0);

            var utilizeState = await _returnedProductUtilizeRepository.GetGroupByModelAsync();
            utilizeState = utilizeState.Where(x => x.Count > 0);

            if (importFromFactoryToBuffer.Any())
            {
                await NotifyTransactions(importFromFactoryToBuffer, $"{from:yyyy-MM-dd} / {to:yyyy-MM-dd} OXANGARON > BUFFER");
            }

            if (exportFromBufferToRepair.Any())
            {
                await NotifyTransactions(exportFromBufferToRepair, $"{from:yyyy-MM-dd} / {to:yyyy-MM-dd} BUFFER > REMONT");
            }

            if (bufferState.Any())
            {
                await NotifProductState(bufferState, $"BUFFERDA MAVJUD MAHSULOTLAR");
            }

            // REPAIR
            if (exportFromRepairToStore.Any())
            {
                await NotifyTransactions(exportFromRepairToStore, $"{from:yyyy-MM-dd} / {to:yyyy-MM-dd} REMONT > OMBOR");
            }

            if (exportFromRepairToUtilize.Any())
            {
                await NotifyTransactions(exportFromRepairToUtilize, $"{from:yyyy-MM-dd} / {to:yyyy-MM-dd} REMONT > OMBOR (UTILIZATSIYA)");
            }

            if (reapirState.Any())
            {
                await NotifProductState(reapirState, $"REMONTDA MAVJUD MAHSULOTLAR");
            }

            // STORE
            if (exportFromStoreToFactory.Any())
            {
                await NotifyTransactions(exportFromStoreToFactory, $"{from:yyyy-MM-dd} / {to:yyyy-MM-dd} OMBOR > OXANGARON");
            }

            if (exportFromStoreToUtilize.Any())
            {
                await NotifyTransactions(exportFromStoreToUtilize, $"{from:yyyy-MM-dd} / {to:yyyy-MM-dd} OMBOR > UTILIZATSIYA");
            }

            if (storeState.Any())
            {
                await NotifProductState(storeState, $"OMBORDA MAVJUD MAHSULOTLAR");
            }

            if (utilizeState.Any())
            {
                await NotifProductState(utilizeState, $"OMBORDA MAVJUD MAHSULOTLAR (UTILIZATSIYA)");
            }
        }

        private async Task NotifyTransactions(IEnumerable<ReturnedProductTransaction> transactions, string title)
        {
            if (transactions == null || !transactions.Any())
            {
                return;
            }

            var html = BuildNotificationTransactionsBody(transactions, title);

            var memoryStream = ConvertHtmlToImage(html);

            await _notificationService.NotifyAsync(memoryStream, title);
        }

        private static string BuildNotificationTransactionsBody(IEnumerable<ReturnedProductTransaction> transactions, string title)
        {
            var builder = new StringBuilder();
            foreach (var readyProduct in transactions)
            {
                builder.AppendLine("<tr>");
                builder.AppendLine($"<td>{readyProduct.Model.Name}</td>");
                builder.AppendLine($"<td>{readyProduct.Model.SapCode}</td>");
                builder.AppendLine($"<td>{readyProduct.Model.Barcode}</td>");
                builder.AppendLine($"<td>{readyProduct.Count}</td>");
                builder.AppendLine("</tr>");
            }

            return $@"<!DOCTYPE html>
                        <html>
                        <head>
                        <style>
                        table {{
                          font-family: arial, sans-serif;
                          border-collapse: collapse;
                          width: 100%;
                        }}

                        td, th {{
                          border: 1px solid #dddddd;
                          text-align: left;
                          padding: 8px;
                        }}
                        </style>
                        </head>
                            <body>
                                <h2>{title}</h2>
                                <table>
                                  <tr>
                                    <th>MODEL</th>
                                    <th>SAP CODE</th>
                                    <th>BAR CODE</th>
                                    <th>SONI</th>
                                  </tr>
                                  {builder}
                                </table>
                            </body>
                        </html>";
        }

        private async Task NotifProductState(IEnumerable<ReturnedProduct> returnedProducts, string title)
        {
            if (returnedProducts == null || !returnedProducts.Any())
            {
                return;
            }

            var html = BuildReturnedProductsNotificationBody(returnedProducts, title);

            var memoryStream = ConvertHtmlToImage(html);

            await _notificationService.NotifyAsync(memoryStream, title);
        }

        private static string BuildReturnedProductsNotificationBody(IEnumerable<ReturnedProduct> readyProductTransactions, string title)
        {
            var builder = new StringBuilder();
            foreach (var readyProduct in readyProductTransactions)
            {
                builder.AppendLine("<tr>");
                builder.AppendLine($"<td>{readyProduct.Model.Name}</td>");
                builder.AppendLine($"<td>{readyProduct.Model.SapCode}</td>");
                builder.AppendLine($"<td>{readyProduct.Model.Barcode}</td>");
                builder.AppendLine($"<td>{readyProduct.Count}</td>");
                builder.AppendLine("</tr>");
            }

            return $@"<!DOCTYPE html>
                        <html>
                        <head>
                        <style>
                        table {{
                          font-family: arial, sans-serif;
                          border-collapse: collapse;
                          width: 100%;
                        }}

                        td, th {{
                          border: 1px solid #dddddd;
                          text-align: left;
                          padding: 8px;
                        }}
                        </style>
                        </head>
                            <body>
                                <h2>{title}</h2>
                                <table>
                                  <tr>
                                    <th>MODEL</th>
                                    <th>SAP CODE</th>
                                    <th>BAR CODE</th>
                                    <th>SONI</th>
                                  </tr>
                                  {builder}
                                </table>
                            </body>
                        </html>";
        }

        private static MemoryStream ConvertHtmlToImage(string html)
        {
            var converter = new HtmlConverter();
            var bytes = converter.FromHtmlString(html);
            return new MemoryStream(bytes);
        }
    }
}
