using SMT.Access.Repository.Interfaces.Service;
using SMT.Access.Unit;
using SMT.Domain.Service;
using SMT.ViewModel.Dto.ServiceCenterRequestDto;
using System;
using System.Threading.Tasks;

namespace SMT.Services.Service
{
    public class ServiceCenterRequestService : IServiceCenterRequestService
    {
        private readonly IServiceCenterRequestRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public ServiceCenterRequestService(IServiceCenterRequestRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ServiceCenterRequest> AddAsync(ServiceCenterRequestCreate serviceCenterRequestCreate)
        {
            var serviceCenterRequest = new ServiceCenterRequest
            {
                CreatedAt = DateTime.Now,
                RequestSenderId = serviceCenterRequestCreate.RequestSenderId,
                ServiceCenterId = serviceCenterRequestCreate.ServiceCenterId,
                MessageId = serviceCenterRequestCreate.MessageId,
            };

            await repository.AddAsync(serviceCenterRequest);
            await unitOfWork.SaveAsync();

            return serviceCenterRequest;
        }
    }
}
