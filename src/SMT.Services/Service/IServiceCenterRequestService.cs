using SMT.Domain.Service;
using SMT.ViewModel.Dto.ServiceCenterRequestDto;
using System.Threading.Tasks;

namespace SMT.Services.Service
{
    public interface IServiceCenterRequestService
    {
        Task<ServiceCenterRequest> AddAsync(ServiceCenterRequestCreate serviceCenterRequestCreate);
    }
}
