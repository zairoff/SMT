using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.ViewModel.Dto.ProductBrandDto;
using SMT.Domain;
using SMT.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMT.Services.Exceptions;

namespace SMT.Services
{
    public class ProductBrandService : IProductBrandService
    {
        private readonly IProductBrandRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductBrandService(IProductBrandRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductBrandResponse> AddAsync(ProductBrandCreate productBrandCreate)
        {
            var productBrand = await _repository.FindAsync(
                            p => p.ProductId == productBrandCreate.ProductId &&
                            p.BrandId == productBrandCreate.BrandId);

            if (productBrand != null)
                throw new ConflictException($"{productBrand.Brand.Name} under {productBrand.Product.Name} already exist");

            productBrand = _mapper.Map<ProductBrandCreate, ProductBrand>(productBrandCreate);

            await _repository.AddAsync(productBrand);
            await _unitOfWork.SaveAsync();

            //var id = productBrand.Id;
            //productBrand = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<ProductBrand, ProductBrandResponse>(productBrand);
        }

        public async Task<ProductBrandResponse> DeleteAsync(int id)
        {
            var productBrand = await _repository.FindAsync(p => p.Id == id);

            if (productBrand == null)
                throw new NotFoundException("Not found");

            _repository.Delete(productBrand);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ProductBrand, ProductBrandResponse>(productBrand);
        }

        public async Task<IEnumerable<ProductBrandResponse>> GetAllAsync()
        {
            var productBrands = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<ProductBrandResponse>>(productBrands);
        }

        public async Task<ProductBrandResponse> GetAsync(int id)
        {
            var productBrand = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<ProductBrand, ProductBrandResponse>(productBrand);
        }

        public async Task<ProductBrandResponse> GetByProductAndBrandIdAsync(int productId, int brandId)
        {
            var productBrand = await _repository.FindAsync(p => p.ProductId == productId && p.BrandId == brandId);

            return _mapper.Map<ProductBrand, ProductBrandResponse>(productBrand);
        }

        public async Task<IEnumerable<ProductBrandResponse>> GetByProductIdAsync(int productId)
        {
            var productBrands = await _repository.GetByAsync(p => p.ProductId == productId);

            return _mapper.Map<IEnumerable<ProductBrand>, IEnumerable<ProductBrandResponse>>(productBrands);
        }

        public async Task<ProductBrandResponse> UpdateAsync(int id, ProductBrandUpdate productBrandUpdate)
        {
            var productBrand = await _repository.FindAsync(p => p.Id == id);

            if (productBrand == null)
                throw new NotFoundException("Not found");

            productBrand.ProductId = productBrandUpdate.ProductId;
            productBrand.BrandId = productBrandUpdate.BrandId;

            _repository.Update(productBrand);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ProductBrand, ProductBrandResponse>(productBrand);
        }
    }
}
