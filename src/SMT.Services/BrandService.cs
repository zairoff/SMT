using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.ViewModel.Exceptions;
using SMT.Domain;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.BrandDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BrandResponse> AddAsync(BrandCreate brandCreate)
        {
            var brand = await _repository.FindAsync(p => p.Name == brandCreate.Name);

            if (brand != null)
                throw new ConflictException($"{brandCreate.Name} already exists");

            brand = _mapper.Map<BrandCreate, Brand>(brandCreate);

            await _repository.AddAsync(brand);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Brand, BrandResponse>(brand);
        }

        public async Task<BrandResponse> DeleteAsync(int id)
        {
            var brand = await _repository.FindAsync(p => p.Id == id);

            if (brand == null)
                throw new NotFoundException("Not found");

            _repository.Delete(brand);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Brand, BrandResponse>(brand);
        }

        public async Task<IEnumerable<BrandResponse>> GetAllAsync()
        {
            var brands = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<Brand>, IEnumerable<BrandResponse>>(brands);
        }

        public async Task<BrandResponse> GetAsync(int id)
        {
            var brand = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<Brand, BrandResponse>(brand);
        }

        public async Task<BrandResponse> GetByNameAsync(string name)
        {
            var brand = await _repository.FindAsync(p => p.Name == name);

            return _mapper.Map<Brand, BrandResponse>(brand);
        }

        public async Task<BrandResponse> UpdateAsync(int id, BrandUpdate brandUpdate)
        {
            var brand = await _repository.FindAsync(p => p.Id == id);

            if (brand == null)
                throw new NotFoundException("Not found");

            brand.Name = brandUpdate.Name;

            _repository.Update(brand);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Brand, BrandResponse>(brand);
        }
    }
}
