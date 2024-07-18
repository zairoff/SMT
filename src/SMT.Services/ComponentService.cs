using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ComponentDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class ComponentService : IComponentService
    {
        private readonly IComponentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ComponentService(IComponentRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ComponentResponse> AddAsync(ComponentCreate componentCreate)
        {
            var component = await _repository.FindAsync(p => p.PartNumber == componentCreate.PartNumber && p.IsActive == true);

            if (component != null)
                throw new ConflictException($"Component {componentCreate.PartNumber} already exists");

            component = _mapper.Map<ComponentCreate, Component>(componentCreate);
            component.IsActive = true;

            await _repository.AddAsync(component);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Component, ComponentResponse>(component);
        }

        public async Task<ComponentResponse> DeleteAsync(int id)
        {
            var component = await _repository.FindAsync(p => p.Id == id);

            if (component == null)
                throw new NotFoundException("Not found");

            component.IsActive = false;

            _repository.Update(component);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Component, ComponentResponse>(component);
        }

        public async Task<IEnumerable<ComponentResponse>> GetAllAsync()
        {
            var components = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<Component>, IEnumerable<ComponentResponse>>(components);
        }

        public async Task<ComponentResponse> GetAsync(int id)
        {
            var component = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<Component, ComponentResponse>(component);
        }

        public async Task<IEnumerable<ComponentResponse>> GetAsync(int page, int pageSize)
        {
            var components = await _repository.GetComponentsAsync(page, pageSize);

            return _mapper.Map<IEnumerable<Component>, IEnumerable<ComponentResponse>>(components);
        }

        public async Task<ComponentResponse> GetByPartNumberAsync(string partNumber)
        {
            var component = await _repository.FindAsync(p => p.PartNumber == partNumber && p.IsActive == true);

            return _mapper.Map<Component, ComponentResponse>(component);
        }

        public async Task<ComponentResponse> GetByRcodeAsync(string rcode)
        {
            var component = await _repository.FindAsync(p => p.RCode == rcode && p.IsActive == true);

            return _mapper.Map<Component, ComponentResponse>(component);
        }

        public async Task<ComponentResponse> GetByStorePlaceAsync(string storePlaceNumber)
        {
            var component = await _repository.FindAsync(p => p.StorePlaceNumber == storePlaceNumber && p.IsActive == true);

            return _mapper.Map<Component, ComponentResponse>(component);
        }

        public async Task<ComponentResponse> UpdateAsync(int id, ComponentUpdate componentUpdate)
        {
            var component = await _repository.FindAsync(p => p.Id == id);

            if (component == null)
                throw new NotFoundException("Not found");

            component.PartNumber = componentUpdate.PartNumber;
            component.RCode = componentUpdate.RCode;
            component.SapPlace = componentUpdate.SapPlace;
            component.StorePlaceNumber = componentUpdate.StorePlaceNumber;
            component.PlaceCode = componentUpdate.PlaceCode;
            component.IsActive = componentUpdate.IsActive;
            component.Specification = componentUpdate.Specification;

            _repository.Update(component);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Component, ComponentResponse>(component);
        }
    }
}
