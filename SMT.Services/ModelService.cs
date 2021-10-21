using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.Access;
using SMT.Common.Dto.ModelDto;
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
    public class ModelService : IModelService
    {
        private readonly IRepository<Model> _repository;
        private readonly IMapper _mapper;

        public ModelService(IRepository<Model> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ModelResponse> AddAsync(ModelCreate modelCreate)
        {
            var model = await _repository.Get().Where(p => p.Name == modelCreate.Name).FirstOrDefaultAsync();

            if (model != null)
                throw new ConflictException();

            model = _mapper.Map<ModelCreate, Model>(modelCreate);

            await _repository.AddAsync(model);

            return _mapper.Map<Model, ModelResponse>(model);
        }

        public async Task<ModelResponse> DeleteAsync(int id)
        {
            var model = await _repository.Get().Where(p => p.Id == id)
                                                .Include(p => p.Product)
                                                .Include(p => p.Brand)
                                                .FirstOrDefaultAsync();

            if (model == null)
                throw new NotFoundException();

            await _repository.DeleteAsync(model);

            return _mapper.Map<Model, ModelResponse>(model);
        }

        public async Task<IEnumerable<ModelResponse>> GetAllAsync()
        {
            var models = await _repository.GetAll()
                                            .Include(p => p.Product)
                                            .Include(p => p.Brand)
                                            .ToListAsync();

            return _mapper.Map<IEnumerable<Model>, IEnumerable<ModelResponse>>(models);
        }

        public async Task<ModelResponse> GetAsync(int id)
        {
            var Model = await _repository.Get().Where(p => p.Id == id)
                                                .Include(p => p.Product)
                                                .Include(p => p.Brand)
                                                .FirstOrDefaultAsync();

            return _mapper.Map<Model, ModelResponse>(Model);
        }

        public async Task<IEnumerable<ModelResponse>> GetByBrandIdAsync(int productId, int brandId)
        {
            var models = await _repository.GetAll()
                                            .Where(m => m.ProductId == productId && m.BrandId == brandId)
                                            .Include(p => p.Product)
                                            .Include(p => p.Brand)
                                            .ToListAsync();

            return _mapper.Map<IEnumerable<Model>, IEnumerable<ModelResponse>>(models);
        }

        public async Task<ModelResponse> GetByNameAsync(string name)
        {
            var model = await _repository.Get().Where(p => p.Name == name)
                                                .Include(p => p.Product)
                                                .Include(p => p.Brand)
                                                .FirstOrDefaultAsync();

            return _mapper.Map<Model, ModelResponse>(model);
        }

        public async Task<ModelResponse> UpdateAsync(int id, ModelUpdate modelUpdate)
        {
            var model = await _repository.Get().Where(p => p.Id == id)
                                                .Include(p => p.Product)
                                                .Include(p => p.Brand)
                                                .FirstOrDefaultAsync();

            if (model == null)
                throw new NotFoundException();

            model.Name = modelUpdate.Name;

            await _repository.UpdateAsync(model);

            return _mapper.Map<Model, ModelResponse>(model);
        }
    }
}
