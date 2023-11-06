using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ReadyProductDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class ReadyProductService : IReadyProductService
    {
        private readonly IReadyProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReadyProductService(IUnitOfWork unitOfWork, IReadyProductRepository repository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ReadyProductResponse> AddAsync(ReadyProductCreate readyProductCreate)
        {
            var readyProduct = _mapper.Map<ReadyProductCreate, ReadyProduct>(readyProductCreate);

            await _repository.AddAsync(readyProduct);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReadyProduct, ReadyProductResponse>(readyProduct);
        }

        public async Task<ReadyProductResponse> DeleteAsync(int id)
        {
            var readyProduct = await _repository.FindAsync(p => p.Id == id);

            if (readyProduct == null)
                throw new NotFoundException("Not found");

            _repository.Delete(readyProduct);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReadyProduct, ReadyProductResponse>(readyProduct);
        }

        public async Task<IEnumerable<ReadyProductResponse>> GetAllAsync()
        {
            var readyProducts = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<ReadyProduct>, IEnumerable<ReadyProductResponse>>(readyProducts);
        }

        public async Task<ReadyProductResponse> GetAsync(int id)
        {
            var readyProduct = await _repository.FindAsync(x => x.Id == id);

            return _mapper.Map<ReadyProduct, ReadyProductResponse>(readyProduct);
        }

        public async Task<IEnumerable<ReadyProductResponse>> GetByEnterDateAsync(DateTime date)
        {
            var readyProducts = await _repository.GetByAsync(x => x.Enter.Date == date.Date);

            return _mapper.Map<IEnumerable<ReadyProduct>, IEnumerable<ReadyProductResponse>>(readyProducts);
        }

        public async Task<IEnumerable<ReadyProductResponse>> GetByExitDateAsync(DateTime date)
        {
            var readyProducts = await _repository.GetByAsync(x => x.Exit.Date == date.Date);

            return _mapper.Map<IEnumerable<ReadyProduct>, IEnumerable<ReadyProductResponse>>(readyProducts);
        }

        public async Task<ReadyProductResponse> UpdateAsync(int id, ReadyProductUpdate readyProductUpdate)
        {
            var readyProduct = await _repository.FindAsync(p => p.Id == id);

            if (readyProduct == null)
                throw new NotFoundException("Not found");

            readyProduct.Count = readyProductUpdate.Count;

            _repository.Update(readyProduct);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReadyProduct, ReadyProductResponse>(readyProduct);
        }

        public async Task<IEnumerable<ReadyProductResponse>> GetByEnterDateRangeAsync(DateTime from, DateTime to)
        {
            var readyProducts = await _repository.GetByAsync(x => x.Enter.Date >= from.Date && x.Enter.Date <= to.Date);

            return _mapper.Map<IEnumerable<ReadyProduct>, IEnumerable<ReadyProductResponse>>(readyProducts);
        }

        public async Task<IEnumerable<ReadyProductResponse>> GetByExitDateRangeAsync(DateTime from, DateTime to)
        {
            var readyProducts = await _repository.GetByAsync(x => x.Exit.Date >= from.Date && x.Exit.Date <= to.Date);

            return _mapper.Map<IEnumerable<ReadyProduct>, IEnumerable<ReadyProductResponse>>(readyProducts);
        }
    }
}
