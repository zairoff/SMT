using SMT.Access.Repository.Interfaces.Service;
using SMT.Access.Unit;
using SMT.Domain.Service;
using SMT.ViewModel.Dto.ServiceCenterRequestSenderDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Service
{
    public class RequestSenderService : IRequestSenderService
    {
        private readonly IRequestSenderRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public RequestSenderService(IRequestSenderRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ServiceCenterRequestSender> AddAsync(RequestSenderCreate requestSenderCreate)
        {
            var requestSender = await repository.FindAsync(x => x.ChatId == requestSenderCreate.ChatId || x.Phone == requestSenderCreate.Phone);

            if (requestSender == null)
            {
                requestSender = new ServiceCenterRequestSender
                {
                    ChatId = requestSenderCreate.ChatId,
                    Phone = requestSenderCreate.Phone,
                    ServiceCenterId = requestSenderCreate.ServiceCenterId,
                };

                await repository.AddAsync(requestSender);
                await unitOfWork.SaveAsync();
            }

            return requestSender;
        }

        public async Task<IEnumerable<ServiceCenterRequestSender>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }
    }
}
