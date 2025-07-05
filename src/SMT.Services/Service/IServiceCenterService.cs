using SMT.ViewModel.Dto.ServiceCenterDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Service
{
    public interface IServiceCenterService
    {
        Task<IEnumerable<ServiceCenterResponse>> GetAllAsync(bool? isActive);

        Task<ServiceCenterResponse> GetAsync(int id);

        Task<ServiceCenterResponse> GetByNameAsync(string name);

        Task<ServiceCenterResponse> AddAsync(ServiceCenterCreate serviceCenterCreate);

        Task<ServiceCenterResponse> UpdateAsync(int id, ServiceCenterUpdate serviceCenterUpdate);

        Task<ServiceCenterResponse> DeleteAsync(int id);
    }
}
