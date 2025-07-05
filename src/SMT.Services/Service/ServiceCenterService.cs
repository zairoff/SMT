using AutoMapper;
using SMT.Access.Repository.Interfaces.Service;
using SMT.Access.Unit;
using SMT.Domain.Service;
using SMT.Services.Exceptions;
using SMT.ViewModel.Dto.ServiceCenterDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Service
{
    public class ServiceCenterService : IServiceCenterService
    {
        private readonly IServiceCenterRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiceCenterService(IServiceCenterRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceCenterResponse> AddAsync(ServiceCenterCreate serviceCenterCreate)
        {
            var serviceCenter = await _repository.FindAsync(p => p.Name == serviceCenterCreate.Name);

            if (serviceCenter != null)
                throw new ConflictException($"{serviceCenterCreate.Name} already exists");

            serviceCenter = _mapper.Map<ServiceCenterCreate, ServiceCenter>(serviceCenterCreate);

            await _repository.AddAsync(serviceCenter);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ServiceCenter, ServiceCenterResponse>(serviceCenter);
        }

        public async Task<ServiceCenterResponse> DeleteAsync(int id)
        {
            var serviceCenter = await _repository.FindAsync(p => p.Id == id);

            if (serviceCenter == null)
                throw new NotFoundException("Not found");

            serviceCenter.IsActive = false;
            _repository.Update(serviceCenter);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<ServiceCenter, ServiceCenterResponse>(serviceCenter);
        }

        public async Task<IEnumerable<ServiceCenterResponse>> GetAllAsync(bool? isActive)
        {
            var serviceCenters = await _repository.GetByAsync(x => x.IsActive == isActive);

            return _mapper.Map<IEnumerable<ServiceCenter>, IEnumerable<ServiceCenterResponse>>(serviceCenters);
        }

        public async Task<ServiceCenterResponse> GetAsync(int id)
        {
            var serviceCenter = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<ServiceCenter, ServiceCenterResponse>(serviceCenter);
        }

        public async Task<ServiceCenterResponse> GetByNameAsync(string name)
        {
            var serviceCenter = await _repository.FindAsync(p => p.Name == name);

            return _mapper.Map<ServiceCenter, ServiceCenterResponse>(serviceCenter);
        }

        public async Task<ServiceCenterResponse> UpdateAsync(int id, ServiceCenterUpdate serviceCenterUpdate)
        {
            var serviceCenter = await _repository.FindAsync(p => p.Id == id);

            if (serviceCenter == null)
                throw new NotFoundException("Not found");

            serviceCenter.Name = serviceCenterUpdate.Name;

            _repository.Update(serviceCenter);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ServiceCenter, ServiceCenterResponse>(serviceCenter);
        }
    }
}
