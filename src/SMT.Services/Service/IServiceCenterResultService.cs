using SMT.Domain.Service;
using SMT.ViewModel.Dto.ServiceCenterResultDto;
using System.Threading.Tasks;

namespace SMT.Services.Service
{
    public interface IServiceCenterResultService
    {
        Task<ServiceCenterResult> AddAsync(ServiceCenterResultStatus serviceCenterResultStatus, ServiceStatus status);

        Task<ServiceCenterResult> GetAsync(long messageId);

    }
}
