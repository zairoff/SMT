using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ComponentRequestDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class ComponentRequestService : IComponentRequestService
    {
        private readonly IComponentRequestRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ComponentRequestService(IComponentRequestRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ComponentRequestResponse> AddAsync(ComponentRequestCreate componentRequestCreate)
        {
            var componentRequest = _mapper.Map<ComponentRequestCreate, ComponentRequest>(componentRequestCreate);

            await _repository.AddAsync(componentRequest);
            await _unitOfWork.SaveAsync();

            componentRequest = await _repository.FindAsync(r => r.Id == componentRequest.Id);

            return _mapper.Map<ComponentRequest, ComponentRequestResponse>(componentRequest);
        }

        public async Task<IEnumerable<ComponentRequestResponse>> GetAllAsync()
        {
            var componentRequests = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<ComponentRequest>, IEnumerable<ComponentRequestResponse>>(componentRequests);
        }

        public async Task<IEnumerable<ComponentRequestResponse>> GetOpenAsync()
        {
            var componentRequests = await _repository.GetOpenAsync();

            return _mapper.Map<IEnumerable<ComponentRequest>, IEnumerable<ComponentRequestResponse>>(componentRequests);
        }
    }
}
