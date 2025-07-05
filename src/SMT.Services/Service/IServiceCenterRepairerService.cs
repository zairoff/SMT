using SMT.Domain.Service;
using SMT.ViewModel.Dto.ServiceCenterRepairerDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Service
{
    public interface IServiceCenterRepairerService
    {
        Task<IEnumerable<ServiceCenterRepairer>> GetAllAsync(bool? isActive);

        Task<ServiceCenterRepairer> GetAsync(int id);

        Task<ServiceCenterRepairer> GetByNameAsync(string name);

        Task<ServiceCenterRepairer> GetByPhoneAsync(string phone);

        Task<ServiceCenterRepairer> GetByChatIdAsync(long chatId);

        Task<ServiceCenterRepairer> GetByTelegramIdAsync(long telegramId);

        Task<ServiceCenterRepairer> AddAsync(ServiceCenterRepairerCreate serviceCenterRepairerCreate);

        Task<ServiceCenterRepairer> DeleteAsync(int id);
    }
}
