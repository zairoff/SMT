using SMT.Access.Repository.Interfaces.Service;
using SMT.Access.Unit;
using SMT.Domain.Service;
using SMT.Services.Exceptions;
using SMT.ViewModel.Dto.ServiceCenterRepairerDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Service
{
    public class ServiceCenterRepairerService : IServiceCenterRepairerService
    {
        private readonly IServiceCenterRepairerRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public ServiceCenterRepairerService(IServiceCenterRepairerRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ServiceCenterRepairer> AddAsync(ServiceCenterRepairerCreate serviceCenterRepairerCreate)
        {
            var serviceCenterRepairer = await repository.FindAsync(p => p.PhoneNumber == serviceCenterRepairerCreate.PhoneNumber || p.TelegramId == serviceCenterRepairerCreate.TelegramId);

            if (serviceCenterRepairer != null)
            {
                if (!serviceCenterRepairer.IsActive)
                {
                    serviceCenterRepairer.IsActive = true;
                    repository.Update(serviceCenterRepairer);
                }

                return serviceCenterRepairer;
            }

            serviceCenterRepairer = new ServiceCenterRepairer
            {
                IsActive = true,
                Name = serviceCenterRepairerCreate.Name,
                PhoneNumber = serviceCenterRepairerCreate.PhoneNumber,
                TelegramId = serviceCenterRepairerCreate.TelegramId,
                ChatId = serviceCenterRepairerCreate.ChatId,
            };

            await repository.AddAsync(serviceCenterRepairer);

            await unitOfWork.SaveAsync();

            return serviceCenterRepairer;
        }

        public async Task<ServiceCenterRepairer> DeleteAsync(int id)
        {
            var serviceCenterRepairer = await repository.FindAsync(p => p.Id == id);

            if (serviceCenterRepairer == null)
                throw new NotFoundException("Not found");

            serviceCenterRepairer.IsActive = false;

            repository.Update(serviceCenterRepairer);
            await unitOfWork.SaveAsync();

            return serviceCenterRepairer;
        }

        public async Task<IEnumerable<ServiceCenterRepairer>> GetAllAsync(bool? isActive)
        {
            var employees = await repository.GetByAsync(x => x.IsActive == isActive);

            return employees;
        }

        public async Task<ServiceCenterRepairer> GetAsync(int id)
        {
            var employee = await repository.FindAsync(x => x.Id == id);

            return employee;
        }

        public Task<ServiceCenterRepairer> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceCenterRepairer> GetByPhoneAsync(string phone)
        {
            var employee = await repository.FindAsync(x => x.PhoneNumber == phone);

            return employee;
        }

        public async Task<ServiceCenterRepairer> GetByChatIdAsync(long chatId)
        {
            var employee = await repository.FindAsync(x => x.ChatId == chatId);

            return employee;
        }

        public async Task<ServiceCenterRepairer> GetByTelegramIdAsync(long telegramId)
        {
            var employee = await repository.FindAsync(x => x.TelegramId == telegramId);

            return employee;
        }
    }
}
