using AutoMapper;
using CoreHtmlToImage;
using SMT.Access.Migrations;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Notification;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ProductTransactionDto;
using SMT.ViewModel.Dto.ReadyProductDto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class ReadyProductService : IReadyProductService
    {
        private readonly IModelRepository _modelRepository;
        private readonly IReadyProductRepository _readyProductRepository;
        private readonly IReadyProductTransactionRepository _transactionRepository;
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReadyProductService(IUnitOfWork unitOfWork, IReadyProductRepository repository, IMapper mapper, IReadyProductTransactionRepository transactionRepository, IModelRepository modelRepository, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _readyProductRepository = repository;
            _mapper = mapper;
            _transactionRepository = transactionRepository;
            _modelRepository = modelRepository;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<ReadyProductResponse>> GetAllAsync()
        {
            var readyProducts = await _readyProductRepository.GetByAsync(x => x.Count > 0);

            return _mapper.Map<IEnumerable<ReadyProduct>, IEnumerable<ReadyProductResponse>>(readyProducts);
        }

        public async Task<ReadyProductResponse> GetAsync(int id)
        {
            var readyProduct = await _readyProductRepository.FindAsync(x => x.Id == id);

            return _mapper.Map<ReadyProduct, ReadyProductResponse>(readyProduct);
        }

        public async Task<IEnumerable<ReadyProductResponse>> GetByProductBrandAsync(int productBrandId)
        {
            var readyProducts =  await _readyProductRepository.GetByAsync(x => x.Model.ProductBrandId == productBrandId && x.Count > 0);

            return _mapper.Map<IEnumerable<ReadyProduct>, IEnumerable<ReadyProductResponse>>(readyProducts);
        }

        public async Task<IEnumerable<ReadyProductResponse>> GetByProductAsync(int productId)
        {
            var readyProducts = await _readyProductRepository.GetByAsync(x => x.Model.ProductBrand.ProductId == productId && x.Count > 0);

            return _mapper.Map<IEnumerable<ReadyProduct>, IEnumerable<ReadyProductResponse>>(readyProducts);
        }

        public async Task<ReadyProductResponse> ImportAsync(ReadyProductCreate readyProductCreate)
        {
            var model = await _modelRepository.FindAsync(x => x.Id == readyProductCreate.ModelId);

            if (model == null)
            {
                throw new NotFoundException("Model not found");
            }

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

            // transaction
            //await NotifyTransaction(transaction, model);

            // all products
            //var readyProducts = await _readyProductRepository.GetByAsync(x => x.Count > 0);

            //await NotifyAllProducts(readyProducts);

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
                ModelId = readyProduct.ModelId,
                Model = readyProduct.Model,
                Count = readyProductUpdate.Count,
                Status = ReadyProductTransactionType.Export,
                Date = DateTime.Now,
            };

            await _transactionRepository.AddAsync(transaction);

            await _unitOfWork.SaveAsync();

            // transaction
            await NotifyTransactions(new List<ReadyProductTransaction> { transaction });

            // all products
            var readyProducts = await _readyProductRepository.GetByAsync(x => x.Count > 0);

            await NotifyAllProducts(readyProducts);

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

            readyProductTransaction.Status = ReadyProductTransactionType.Deleted;
            _transactionRepository.Update(readyProductTransaction);

            var readyProduct = await _readyProductRepository.FindAsync(x => x.ModelId == readyProductTransaction.ModelId);

            if (readyProductTransaction == null)
                throw new NotFoundException($"Ready product not found");

            readyProduct.Count -= readyProductTransaction.Count;
            _readyProductRepository.Update(readyProduct);

            await _unitOfWork.SaveAsync();

            //await NotifyTransaction(readyProductTransaction, readyProduct.Model);

            // all products
            //var readyProducts = await _readyProductRepository.GetByAsync(x => x.Count > 0);

            //await NotifyAllProducts(readyProducts);

            return _mapper.Map<ReadyProductTransaction, ReadyProductTransactionResponse>(readyProductTransaction);
        }

        public async Task NotifyAsync()
        {
            var imports = await _transactionRepository.GetByAsync(x => x.Status == ReadyProductTransactionType.Import && x.Date >= DateTime.Now.AddMinutes(-30) && x.Date <= DateTime.Now);
            var exports = await _transactionRepository.GetByAsync(x => x.Status == ReadyProductTransactionType.Export && x.Date >= DateTime.Now.AddMinutes(-30) && x.Date <= DateTime.Now);
            var readyProducts = await _readyProductRepository.GetByAsync(x => x.Count > 0);

            if (imports.Any())
            {
                await NotifyTransactions(imports);
            }

            if (exports.Any())
            {
                await NotifyTransactions(exports);
            }

            if (readyProducts.Any() && (imports.Any() || exports.Any()))
            {
                await NotifyAllProducts(readyProducts);
            }
        }

        private async Task NotifyTransactions(IEnumerable<ReadyProductTransaction> transactions)
        {
            if (transactions == null || !transactions.Any())
            {
                return;
            }

            var title = BuilTitle(transactions.First());

            var html = BuildNotificationTransactionsBody(transactions, title);

            var memoryStream = ConvertHtmlToImage(html);

            await _notificationService.NotifyAsync(memoryStream, title);
        }

        private static string BuilTitle(ReadyProductTransaction transaction)
        {
            if (transaction.Status == ReadyProductTransactionType.Import)
            {
                return "OMBORGA KIRDI";
            }

            if (transaction.Status == ReadyProductTransactionType.Export)
            {
                return "OMBORDAN CHIQDI";
            }

            return "OMBORGA KIRILGAN O'CHIRILDI, SABAB: OPERATOR XATOLIGI";
        }

        private static string BuildNotificationTransactionsBody(IEnumerable<ReadyProductTransaction> transactions, string title)
        {
            var builder = new StringBuilder();
            foreach (var readyProduct in transactions)
            {
                builder.AppendLine("<tr>");
                builder.AppendLine($"<td>{readyProduct.Model.Name}</td>");
                builder.AppendLine($"<td>{readyProduct.Model.SapCode}</td>");
                builder.AppendLine($"<td>{readyProduct.Date}</td>");
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
                                    <th>SANA</th>
                                    <th>SONI</th>
                                  </tr>
                                  {builder}
                                </table>
                            </body>
                        </html>";
        }

        private async Task NotifyAllProducts(IEnumerable<ReadyProduct> readyProducts)
        {
            if (readyProducts == null || !readyProducts.Any())
            {
                return;
            }

            var html = BuildReadyProductsNotificationBody(readyProducts);

            var memoryStream = ConvertHtmlToImage(html);

            await _notificationService.NotifyAsync(memoryStream, "OMBORDA MAVJUD MODELLAR");
        }

        private static string BuildReadyProductsNotificationBody(IEnumerable<ReadyProduct> readyProducts)
        {
            var builder = new StringBuilder();
            foreach (var readyProduct in readyProducts)
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
