using AutoMapper;
using SMT.Access.UnitOfWork;
using SMT.Common.Dto.ModelDto;
using SMT.Common.Exceptions;
using SMT.Domain;
using SMT.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMT.Access.Repository.Interfaces;

namespace SMT.Services
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ModelService(IMapper mapper, IUnitOfWork unitOfWork, IModelRepository repository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<ModelResponse> AddAsync(ModelCreate modelCreate)
        {
            var model = await _repository.FindAsync(p => p.Name == modelCreate.Name);

            if (model != null)
                throw new ConflictException();

            model = _mapper.Map<ModelCreate, Model>(modelCreate);

            await _repository.AddAsync(model);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Model, ModelResponse>(model);
        }

        public async Task<ModelResponse> DeleteAsync(int id)
        {
            var model = await _repository.FindAsync(p => p.Id == id);

            if (model == null)
                throw new NotFoundException();

            _repository.Delete(model);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Model, ModelResponse>(model);
        }

        public async Task<IEnumerable<ModelResponse>> GetAllAsync()
        {
            var models = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<Model>, IEnumerable<ModelResponse>>(models);
        }

        public async Task<ModelResponse> GetAsync(int id)
        {
            var Model = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<Model, ModelResponse>(Model);
        }

        public async Task<IEnumerable<ModelResponse>> GetByProductBrandId(int productBrandId)
        {
            var models = await _repository.GetByAsync(m => m.ProductBrandId == productBrandId);

            return _mapper.Map<IEnumerable<Model>, IEnumerable<ModelResponse>>(models);
        }

        public async Task<ModelResponse> GetByNameAsync(string name)
        {
            var model = await _repository.FindAsync(p => p.Name == name);

            return _mapper.Map<Model, ModelResponse>(model);
        }

        public async Task<ModelResponse> UpdateAsync(int id, ModelUpdate modelUpdate)
        {
            var model = await _repository.FindAsync(p => p.Id == id);


            if (model == null)
                throw new NotFoundException();

            model.Name = modelUpdate.Name;

            _repository.Update(model);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Model, ModelResponse>(model);
        }
    }
}
