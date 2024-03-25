using AutoMapper;
using CoreHtmlToImage;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Notification;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ProductTransactionDto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class ReadyProductTransactionService : IReadyProductTransactionService
    {
        private readonly IModelRepository _modelRepository;
        private readonly IReadyProductTransactionRepository _transactionRepository;
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReadyProductTransactionService(IMapper mapper, IUnitOfWork unitOfWork, INotificationService notificationService, IReadyProductTransactionRepository transactionRepository, IModelRepository modelRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _transactionRepository = transactionRepository;
            _modelRepository = modelRepository;
        }

        public async Task<IEnumerable<ReadyProductTransactionResponse>> GetAllAsync()
        {
            var readyProducts = await _transactionRepository.GetGroupByModelAsync();

            var filteredProducts = readyProducts.Where(x => x.Count > 0);

            return _mapper.Map<IEnumerable<ReadyProductTransaction>, IEnumerable<ReadyProductTransactionResponse>>(filteredProducts);
        }

        public async Task<IEnumerable<ReadyProductTransactionResponse>> GetByProductBrandAsync(int productBrandId)
        {
            var readyProducts = await _transactionRepository.GetGroupByModelAsync(x => x.Model.ProductBrandId == productBrandId);

            var filteredProducts = readyProducts.Where(x => x.Count > 0);

            return _mapper.Map<IEnumerable<ReadyProductTransaction>, IEnumerable<ReadyProductTransactionResponse>>(filteredProducts);
        }

        public async Task<IEnumerable<ReadyProductTransactionResponse>> GetByProductAsync(int productId)
        {
            var readyProducts = await _transactionRepository.GetGroupByModelAsync(x => x.Model.ProductBrand.ProductId == productId);

            var filteredProducts = readyProducts.Where(x => x.Count > 0);

            return _mapper.Map<IEnumerable<ReadyProductTransaction>, IEnumerable<ReadyProductTransactionResponse>>(filteredProducts);
        }

        public async Task<IEnumerable<ReadyProductTransactionResponse>> GetBySapCodeDateRange(string sapCode, DateTime from, DateTime to, ViewModel.Dto.ProductTransactionDto.TransactionType transactionType)
        {
            var productTransactionType = (Domain.ReadyProductTransactionType)transactionType;

            IEnumerable<ReadyProductTransaction> readyProducts;

            if (productTransactionType == Domain.ReadyProductTransactionType.All)
            {
                readyProducts = await _transactionRepository.GetByAsync(x => x.Model.SapCode == sapCode && x.Date.Date >= from.Date && x.Date.Date <= to);
            }
            else
            {
                readyProducts = await _transactionRepository.GetByAsync(x => x.Model.SapCode == sapCode && x.Date.Date >= from.Date && x.Date.Date <= to && x.Status == productTransactionType);
            }

            return _mapper.Map<IEnumerable<ReadyProductTransaction>, IEnumerable<ReadyProductTransactionResponse>>(readyProducts);
        }

        public async Task<ReadyProductTransactionResponse> DeleteTransactionAsync(int id)
        {
            var transaction = await _transactionRepository.FindAsync(p => p.Id == id);

            if (transaction == null)
            {
                throw new InvalidOperationException("Not found");
            }

            transaction.Count = 0;
            transaction.Status = Domain.ReadyProductTransactionType.Deleted;

            _transactionRepository.Update(transaction);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReadyProductTransaction, ReadyProductTransactionResponse>(transaction);
        }

        public async Task<ReadyProductTransactionResponse> ExportAsync(ReadyProductTransactionExport readyProductTransactionExport)
        {
            var count = await _transactionRepository.FindSumAsync(p => p.ModelId == readyProductTransactionExport.ModelId);

            if (count < readyProductTransactionExport.Count)
            {
                throw new InvalidOperationException("Not enough");
            }

            var transaction = new ReadyProductTransaction
            {
                ModelId = readyProductTransactionExport.ModelId,
                Count = -readyProductTransactionExport.Count,
                Status = Domain.ReadyProductTransactionType.Export,
                Date = DateTime.Now,
            };

            await _transactionRepository.AddAsync(transaction);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReadyProductTransaction, ReadyProductTransactionResponse>(transaction);
        }

        public async Task<ReadyProductTransactionResponse> ImportAsync(ReadyProductTransactionImport readyProductTransactionImport)
        {
            var model = await _modelRepository.FindAsync(x => x.Id == readyProductTransactionImport.ModelId);

            if (model == null)
            {
                throw new NotFoundException("Model not found");
            }

            var transaction = new ReadyProductTransaction
            {
                ModelId = readyProductTransactionImport.ModelId,
                Count = readyProductTransactionImport.Count,
                Status = Domain.ReadyProductTransactionType.Import,
                Date = DateTime.Now,
            };

            await _transactionRepository.AddAsync(transaction);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReadyProductTransaction, ReadyProductTransactionResponse>(transaction);
        }

        public async Task<IEnumerable<ReadyProductTransactionResponse>> GetByDateAsync(DateTime date, ViewModel.Dto.ProductTransactionDto.TransactionType transactionType)
        {
            var productTransactionType = (Domain.ReadyProductTransactionType)transactionType;

            IEnumerable<ReadyProductTransaction> readyProducts;

            if (productTransactionType == Domain.ReadyProductTransactionType.All)
            {
                readyProducts = await _transactionRepository.GetByAsync(x => x.Date.Date == date.Date);
            }
            else
            {
                readyProducts = await _transactionRepository.GetByAsync(x => x.Date.Date == date.Date && x.Status == productTransactionType);
            }

            return _mapper.Map<IEnumerable<ReadyProductTransaction>, IEnumerable<ReadyProductTransactionResponse>>(readyProducts);
        }

        public async Task<IEnumerable<ReadyProductTransactionResponse>> GetByDateGroupByAsync(DateTime date, ViewModel.Dto.ProductTransactionDto.TransactionType transactionType)
        {
            var productTransactionType = (Domain.ReadyProductTransactionType)transactionType;

            IEnumerable<ReadyProductTransaction> readyProducts = await _transactionRepository.GetGroupByModelAsync(x => x.Date.Date == date.Date && x.Status == productTransactionType);

            return _mapper.Map<IEnumerable<ReadyProductTransaction>, IEnumerable<ReadyProductTransactionResponse>>(readyProducts);
        }

        public async Task<IEnumerable<ReadyProductTransactionResponse>> GetByDateRangeAsync(DateTime from, DateTime to, ViewModel.Dto.ProductTransactionDto.TransactionType transactionType)
        {
            var productTransactionType = (Domain.ReadyProductTransactionType)transactionType;

            IEnumerable<ReadyProductTransaction> readyProducts;

            if (productTransactionType == Domain.ReadyProductTransactionType.All)
            {
                readyProducts = await _transactionRepository.GetByAsync(x => x.Date.Date >= from.Date && x.Date.Date <= to);
            }
            else
            {
                readyProducts = await _transactionRepository.GetByAsync(x => x.Date.Date >= from.Date && x.Date.Date <= to && x.Status == productTransactionType);
            }

            return _mapper.Map<IEnumerable<ReadyProductTransaction>, IEnumerable<ReadyProductTransactionResponse>>(readyProducts);
        }

        public async Task<IEnumerable<ReadyProductTransactionResponse>> GetByDateRangeGroupByAsync(DateTime from, DateTime to, ViewModel.Dto.ProductTransactionDto.TransactionType transactionType)
        {
            var productTransactionType = (Domain.ReadyProductTransactionType)transactionType;

            var readyProducts = await _transactionRepository.GetGroupByModelAsync(x => x.Date.Date >= from.Date && x.Date.Date <= to && x.Status == productTransactionType);


            return _mapper.Map<IEnumerable<ReadyProductTransaction>, IEnumerable<ReadyProductTransactionResponse>>(readyProducts);
        }

        public async Task GroupByNotifyAsync()
        {
            var imports = await _transactionRepository.GetGroupByModelAsync(x => x.Status == Domain.ReadyProductTransactionType.Import && x.Date.Date == DateTime.Now.Date);
            var exports = await _transactionRepository.GetGroupByModelAsync(x => x.Status == Domain.ReadyProductTransactionType.Export && x.Date.Date == DateTime.Now.Date);
            var readyProducts = await _transactionRepository.GetGroupByModelAsync();
            var filteredProducts = readyProducts.Where(x => x.Count > 0);

            if (imports.Any())
            {
                await NotifyTransactions(imports, $"{DateTime.Now:yyyy:MM:dd} SANADA OMBORGA KIRGAN MAHSULOTLAR");
            }

            if (exports.Any())
            {
                var sortedExports = exports.OrderBy(i => i.Count).ToList();
                sortedExports.ForEach(x => x.Count *= (-1));

                await NotifyTransactions(sortedExports, $"{DateTime.Now:yyyy:MM:dd} SANADA OMBORDAN CHIQGAN MAHSULOTLAR");
            }

            if (filteredProducts.Any() && (imports.Any() || exports.Any()))
            {
                await NotifyAllProducts(filteredProducts);
            }
        }

        public async Task<ReadyProductTransactionResponse> ChangesAsync(int id, int count)
        {
            var transaction = await _transactionRepository.FindAsync(p => p.Id == id);

            if (transaction == null)
            {
                throw new InvalidOperationException("Not found");
            }

            count = transaction.Count < 0 ? -count : count;

            transaction.Count = count;

            _transactionRepository.Update(transaction);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReadyProductTransaction, ReadyProductTransactionResponse>(transaction);
        }

        private async Task NotifyTransactions(IEnumerable<ReadyProductTransaction> transactions, string title)
        {
            if (transactions == null || !transactions.Any())
            {
                return;
            }

            var html = BuildNotificationTransactionsBody(transactions, title);

            var memoryStream = ConvertHtmlToImage(html);

            await _notificationService.NotifyAsync(memoryStream, title);
        }

        private static string BuildNotificationTransactionsBody(IEnumerable<ReadyProductTransaction> transactions, string title)
        {
            var builder = new StringBuilder();
            foreach (var readyProduct in transactions)
            {
                builder.AppendLine("<tr>");
                builder.AppendLine($"<td>{readyProduct.Model.Name}</td>");
                builder.AppendLine($"<td>{readyProduct.Model.SapCode}</td>");
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
                                    <th>SONI</th>
                                  </tr>
                                  {builder}
                                </table>
                            </body>
                        </html>";
        }

        private async Task NotifyAllProducts(IEnumerable<ReadyProductTransaction> readyProductTransactions)
        {
            if (readyProductTransactions == null || !readyProductTransactions.Any())
            {
                return;
            }

            var html = BuildReadyProductsNotificationBody(readyProductTransactions);

            var memoryStream = ConvertHtmlToImage(html);

            await _notificationService.NotifyAsync(memoryStream, "OMBORDA MAVJUD MAHSULOTLAR");
        }

        private static string BuildReadyProductsNotificationBody(IEnumerable<ReadyProductTransaction> readyProductTransactions)
        {
            var builder = new StringBuilder();
            foreach (var readyProduct in readyProductTransactions)
            {
                builder.AppendLine("<tr>");
                builder.AppendLine($"<td>{readyProduct.Model.Name}</td>");
                builder.AppendLine($"<td>{readyProduct.Model.SapCode}</td>");
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
                                <h2>OMBORDA MAVJUD MODELLAR</h2>
                                <table>
                                  <tr>
                                    <th>MODEL</th>
                                    <th>SAP CODE</th>
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
