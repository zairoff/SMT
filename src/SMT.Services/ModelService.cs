using AutoMapper;
using SMT.Access.Unit;
using SMT.ViewModel.Dto.ModelDto;
using SMT.Domain;
using SMT.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMT.Access.Repository.Interfaces;
using SMT.Services.Exceptions;

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
            var model = await _repository.FindAsync(p => p.Name == modelCreate.Name || p.SapCode == modelCreate.SapCode || 
            (p.Barcode == modelCreate.Barcode && !string.IsNullOrEmpty(modelCreate.Barcode)));

            if (model != null)
                throw new ConflictException($"{modelCreate.Name} already exist");

            model = _mapper.Map<ModelCreate, Model>(modelCreate);

            await _repository.AddAsync(model);
            await _unitOfWork.SaveAsync();

            model = await _repository.FindAsync(m => m.Id == model.Id);

            return _mapper.Map<Model, ModelResponse>(model);
        }

        public async Task<ModelResponse> DeleteAsync(int id)
        {
            var model = await _repository.FindAsync(p => p.Id == id);

            if (model == null)
                throw new NotFoundException("Model not found");

            model.IsActive = false;

            _repository.Update(model);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Model, ModelResponse>(model);
        }

        public async Task<IEnumerable<ModelResponse>> GetAllAsync(bool? isActive)
        {
            var models = await _repository.GetByAsync(x => x.IsActive == isActive);

            return _mapper.Map<IEnumerable<Model>, IEnumerable<ModelResponse>>(models);
        }

        public async Task<ModelResponse> GetAsync(int id)
        {
            var Model = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<Model, ModelResponse>(Model);
        }

        public async Task<IEnumerable<ModelResponse>> GetByProductBrandId(int productBrandId, bool? isActive)
        {
            var models = await _repository.GetByAsync(m => m.ProductBrandId == productBrandId && m.IsActive == isActive);

            return _mapper.Map<IEnumerable<Model>, IEnumerable<ModelResponse>>(models);
        }

        public async Task<ModelResponse> GetByNameAsync(string name)
        {
            var model = await _repository.FindAsync(p => p.Name == name);

            return _mapper.Map<Model, ModelResponse>(model);
        }

        public async Task<ModelResponse> UpdateAsync(int id, ModelUpdate modelUpdate)
        {
            _ = modelUpdate.SapCode?.Trim();
            _ = modelUpdate.Barcode?.Trim();

            var model = await _repository.FindAsync(x => (x.SapCode == modelUpdate.SapCode || (x.Barcode == modelUpdate.Barcode && !string.IsNullOrEmpty(modelUpdate.Barcode)) && x.Id != id));

            if (model != null)
                throw new ConflictException($"{modelUpdate.SapCode} already existed");

            model = await _repository.FindAsync(p => p.Id == id);

            if (model == null)
                throw new NotFoundException("Not found");

            model.Name = modelUpdate.Name;
            model.Barcode = modelUpdate.Barcode;
            model.SapCode = modelUpdate.SapCode;

            _repository.Update(model);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Model, ModelResponse>(model);
        }

        public async Task<ModelResponse> GetBySapCodeAsync(string sapCode, bool isActive)
        {
            if (string.IsNullOrEmpty(sapCode))
            {
                return null;
            }

            var model = await _repository.FindAsync(p => p.SapCode == sapCode && p.IsActive == isActive);

            return _mapper.Map<Model, ModelResponse>(model);
        }

        public async Task<ModelResponse> GetByBarcodeAsync(string barcode, bool isActive)
        {
            if (string.IsNullOrEmpty(barcode))
            {
                return null;
            }

            var model = await _repository.FindAsync(p => p.Barcode == barcode && p.IsActive == isActive);

            return _mapper.Map<Model, ModelResponse>(model);
        }
    }
}
