using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.Access.Repository;
using SMT.Common.Dto.ProductBrandDto;
using SMT.Common.Exceptions;
using SMT.Domain;
using SMT.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class ProductBrandService : IProductBrandService
    {
        private readonly IBaseRepository<ProductBrand> _repository;
        private readonly IMapper _mapper;

        public ProductBrandService(IBaseRepository<ProductBrand> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductBrandResponse> AddAsync(ProductBrandCreate productBrandCreate)
        {
            var productBrand = await _repository.Get()
                            .Where(p => p.ProductId == productBrandCreate.ProductId &&
                            p.BrandId == productBrandCreate.BrandId)
                            .FirstOrDefaultAsync();

            if (productBrand != null)
                throw new ConflictException();

            productBrand = _mapper.Map<ProductBrandCreate, ProductBrand>(productBrandCreate);

            await _repository.AddAsync(productBrand);

            return _mapper.Map<ProductBrand, ProductBrandResponse>(productBrand);
        }

        public async Task<ProductBrandResponse> DeleteAsync(int id)
        {
            var productBrand = await _repository.Get()
                                    .Where(p => p.Id == id)
                                    .Include(p => p.Product)
                                    .Include(p => p.Brand)
                                    .FirstOrDefaultAsync();

            if (productBrand == null)
                throw new NotFoundException();

            await _repository.DeleteAsync(productBrand);

            return _mapper.Map<ProductBrand, ProductBrandResponse>(productBrand);
        }

        public async Task<IEnumerable<ProductBrandResponse>> GetAllAsync()
        {
            var productBrands = await _repository.GetAll()
                                                .Include(p => p.Product)
                                                .Include(p => p.Brand)
                                                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<ProductBrandResponse>>(productBrands);
        }

        public async Task<ProductBrandResponse> GetAsync(int id)
        {
            var productBrand = await _repository.Get().Where(p => p.Id == id)
                                                .Include(p => p.Product)
                                                .Include(p => p.Brand)
                                                .FirstOrDefaultAsync();

            return _mapper.Map<ProductBrand, ProductBrandResponse>(productBrand);
        }

        public async Task<IEnumerable<ProductBrandResponse>> GetByProductIdAsync(int productId)
        {
            var productBrands = await _repository.Get()
                                    .Where(p => p.ProductId == productId)
                                    .Include(p => p.Product)
                                    .Include(p => p.Brand)
                                    .ToListAsync();

            return _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<ProductBrandResponse>>(productBrands);
        }

        public async Task<ProductBrandResponse> UpdateAsync(int id, ProductBrandUpdate productBrandUpdate)
        {
            var productBrand = await _repository.Get()
                                            .Where(p => p.Id == id)
                                            .Include(p => p.Product)
                                            .Include(p => p.Brand)
                                            .FirstOrDefaultAsync();

            if (productBrand == null)
                throw new NotFoundException();

            productBrand.ProductId = productBrandUpdate.ProductId;
            productBrand.BrandId = productBrandUpdate.BrandId;

            await _repository.UpdateAsync(productBrand);

            return _mapper.Map<ProductBrand, ProductBrandResponse>(productBrand);
        }
    }
}
