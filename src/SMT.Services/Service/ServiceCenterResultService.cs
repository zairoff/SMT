using SMT.Access.Repository.Interfaces.Service;
using SMT.Access.Unit;
using SMT.Domain.Service;
using SMT.Services.Exceptions;
using SMT.ViewModel.Dto.ServiceCenterResultDto;
using System;
using System.Threading.Tasks;

namespace SMT.Services.Service
{
    public class ServiceCenterResultService : IServiceCenterResultService
    {
        private readonly IServiceCenterRequestRepository serviceCenterRequestRepository;
        private readonly IServiceCenterResultRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public ServiceCenterResultService(IServiceCenterRequestRepository serviceCenterRequestRepository, IServiceCenterResultRepository repository, IUnitOfWork unitOfWork)
        {
            this.serviceCenterRequestRepository = serviceCenterRequestRepository;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ServiceCenterResult> AddAsync(ServiceCenterResultStatus serviceCenterResultStatus, ServiceStatus status)
        {
            var serviceCenterRequest = await serviceCenterRequestRepository.FindAsync(x => x.MessageId == serviceCenterResultStatus.MessageId);

            if (serviceCenterRequest == null)
            {
                throw new NotFoundException("Request Not Found");
            }

            var serviceCenterResult = new ServiceCenterResult
            {
                ServiceStatus = status,
                CreatedAt = DateTime.UtcNow,
                ServiceCenterRepairerId = serviceCenterResultStatus.ServiceCenterRepairerId,
                ServiceCenterRequestId = serviceCenterRequest.Id,
            };

            await repository.AddAsync(serviceCenterResult);
            await unitOfWork.SaveAsync();

            return serviceCenterResult;
        }

        public async Task<ServiceCenterResult> GetAsync(long messageId)
        {
            var serviceCenterRequest = await serviceCenterRequestRepository.FindAsync(x => x.MessageId == messageId);

            if (serviceCenterRequest == null)
            {
                throw new NotFoundException("Request Not Found");
            }

            return await repository.FindAsync(x => x.ServiceCenterRequestId == serviceCenterRequest.Id);
        }

    }
}
