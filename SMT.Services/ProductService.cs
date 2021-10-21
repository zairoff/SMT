using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.Access;
using SMT.Common.Dto.ProductDto;
using SMT.Common.Exceptions;
using SMT.Domain;
using SMT.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _repository;
        private readonly IMapper _mapper;

        public ProductService(IRepository<Product> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductResponse> AddAsync(ProductCreate productCreate)
        {
            var product = await _repository.Get().Where(p => p.Name == productCreate.Name).FirstOrDefaultAsync();

            if (product != null)
                throw new ConflictException();

            product = _mapper.Map<ProductCreate, Product>(productCreate);

            await _repository.AddAsync(product);

            return _mapper.Map<Product, ProductResponse>(product);
        }

        public async Task<ProductResponse> DeleteAsync(int id)
        {
            var product = await _repository.Get().Where(p => p.Id == id).FirstOrDefaultAsync();

            if (product == null)
                throw new NotFoundException();

            await _repository.DeleteAsync(product);

            return _mapper.Map<Product, ProductResponse>(product);
        }

        public async Task<IEnumerable<ProductResponse>> GetAllAsync()
        {
            var products = await _repository.GetAll().ToListAsync();

            return _mapper.Map<IEnumerable<Product>, IEnumerable<ProductResponse>>(products);
        }

        public async Task<ProductResponse> GetAsync(int id)
        {
            var product = await _repository.Get().Where(p => p.Id == id).FirstOrDefaultAsync();

            return _mapper.Map<Product, ProductResponse>(product);
        }

        public async Task<ProductResponse> GetByNameAsync(string name)
        {
            var product = await _repository.Get().Where(p => p.Name == name).FirstOrDefaultAsync();

            return _mapper.Map<Product, ProductResponse>(product);
        }

        public async Task<ProductResponse> UpdateAsync(int id, ProductUpdate productUpdate)
        {
            var product = await _repository.Get().Where(p => p.Id == id).FirstOrDefaultAsync();

            if (product == null)
                throw new NotFoundException();

            product.Name = productUpdate.Name;

            await _repository.UpdateAsync(product);

            return _mapper.Map<Product, ProductResponse>(product);
        }
    }
}
