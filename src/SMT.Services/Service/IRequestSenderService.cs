using SMT.Domain.Service;
using SMT.ViewModel.Dto.ServiceCenterRequestSenderDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Service
{
    public interface IRequestSenderService
    {
        Task<IEnumerable<ServiceCenterRequestSender>> GetAllAsync();

        Task<ServiceCenterRequestSender> AddAsync(RequestSenderCreate requestSenderCreate);
    }
}
