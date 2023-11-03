using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.ViewModel.Dto.ProductDto;
using SMT.Domain;
using SMT.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMT.Services.Exceptions;

namespace SMT.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductResponse> AddAsync(ProductCreate productCreate)
        {
            var product = await _repository.FindAsync(p => p.Name == productCreate.Name);

            if (product != null)
                throw new ConflictException($"{productCreate.Name} alredy exists");

            product = _mapper.Map<ProductCreate, Product>(productCreate);

            await _repository.AddAsync(product);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Product, ProductResponse>(product);
        }

        public async Task<ProductResponse> DeleteAsync(int id)
        {
            var product = await _repository.FindAsync(p => p.Id == id);

            if (product == null)
                throw new NotFoundException("Product not found");

            product.IsActive = false;

            _repository.Update(product);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Product, ProductResponse>(product);
        }

        public async Task<IEnumerable<ProductResponse>> GetAllAsync(bool? isActive)
        {
            var products = await _repository.GetByAsync(x => x.IsActive == isActive);

            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponse>>(products);
        }

        public async Task<ProductResponse> GetAsync(int id)
        {
            var product = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<Product, ProductResponse>(product);
        }

        public async Task<ProductResponse> GetByNameAsync(string name)
        {
            var product = await _repository.FindAsync(p => p.Name == name);

            return _mapper.Map<Product, ProductResponse>(product);
        }

        public async Task<ProductResponse> UpdateAsync(int id, ProductUpdate productUpdate)
        {
            var product = await _repository.FindAsync(p => p.Id == id);

            if (product == null)
                throw new NotFoundException("Product not found");

            product.Name = productUpdate.Name;

            _repository.Update(product);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Product, ProductResponse>(product);
        }
    }
}
