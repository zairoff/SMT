using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.Access;
using SMT.Common.Dto.BrandDto;
using SMT.Common.Exceptions;
using SMT.Domain;
using SMT.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class BrandService : IBrandService
    {
        private readonly IRepository<Brand> _repository;
        private readonly IMapper _mapper;

        public BrandService(IRepository<Brand> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BrandResponse> AddAsync(BrandCreate brandCreate)
        {
            var brand = await _repository.Get().Where(p => p.Name == brandCreate.Name).FirstOrDefaultAsync();

            if (brand != null)
                throw new ConflictException();

            brand = _mapper.Map<BrandCreate, Brand>(brandCreate);

            await _repository.AddAsync(brand);

            return _mapper.Map<Brand, BrandResponse>(brand);
        }

        public async Task<BrandResponse> DeleteAsync(int id)
        {
            var brand = await _repository.Get().Where(p => p.Id == id).FirstOrDefaultAsync();

            if (brand == null)
                throw new NotFoundException();

            await _repository.DeleteAsync(brand);

            return _mapper.Map<Brand, BrandResponse>(brand);
        }

        public async Task<IEnumerable<BrandResponse>> GetAllAsync()
        {
            var brands = await _repository.GetAll().ToListAsync();

            return _mapper.Map<IEnumerable<Brand>, IEnumerable<BrandResponse>>(brands);
        }

        public async Task<BrandResponse> GetAsync(int id)
        {
            var brand = await _repository.Get().Where(p => p.Id == id).FirstOrDefaultAsync();

            return _mapper.Map<Brand, BrandResponse>(brand);
        }

        public async Task<BrandResponse> GetByNameAsync(string name)
        {
            var brand = await _repository.Get().Where(p => p.Name == name).FirstOrDefaultAsync();

            return _mapper.Map<Brand, BrandResponse>(brand);
        }

        public async Task<BrandResponse> UpdateAsync(int id, BrandUpdate brandUpdate)
        {
            var brand = await _repository.Get().Where(p => p.Id == id).FirstOrDefaultAsync();

            if (brand == null)
                throw new NotFoundException();

            brand.Name = brandUpdate.Name;

            await _repository.UpdateAsync(brand);

            return _mapper.Map<Brand, BrandResponse>(brand);
        }
    }
}
