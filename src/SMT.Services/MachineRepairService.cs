using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Notification;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.MachineRepairDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class MachineRepairService : IMachineRepairService
    {
        private readonly IMachineRepairRepository _repository;
        private readonly INotificationService _notificationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MachineRepairService(IMachineRepairRepository repository, IMapper mapper, IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<MachineRepairResponse> AddAsync(MachineRepairCreate machinerRepairCreate)
        {
            var machineRepair = _mapper.Map<MachineRepairCreate, MachineRepair>(machinerRepairCreate);

            await _repository.AddAsync(machineRepair);
            await _unitOfWork.SaveAsync();

            machineRepair = await _repository.FindAsync(r => r.Id == machineRepair.Id);

            await _notificationService.NotifyRepairAsync(machineRepair);

            return _mapper.Map<MachineRepair, MachineRepairResponse>(machineRepair);
        }

        public async Task<MachineRepairResponse> DeleteAsync(int id)
        {
            var machineRepair = await _repository.FindAsync(p => p.Id == id);

            if (machineRepair == null)
                throw new NotFoundException("Not found");

            _repository.Delete(machineRepair);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<MachineRepair, MachineRepairResponse>(machineRepair);
        }

        public async Task<IEnumerable<MachineRepairResponse>> GetAllAsync(string shift)
        {
            IEnumerable<MachineRepair> machineRepairs = Enumerable.Empty<MachineRepair>();
            if (string.IsNullOrEmpty(shift))
            {
                machineRepairs = await _repository.GetAllAsync();
            }
            else
            {
                machineRepairs = await _repository.GetByAsync(x => x.Shift == shift);
            }

            return _mapper.Map<IEnumerable<MachineRepair>, IEnumerable<MachineRepairResponse>>(machineRepairs);
        }

        public async Task<MachineRepairResponse> GetAsync(int id)
        {
            var machineRepair = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<MachineRepair, MachineRepairResponse>(machineRepair);
        }

        public async Task<IEnumerable<MachineRepairResponse>> GetByMachineIdAsync(int machineId, string shift)
        {
            IEnumerable<MachineRepair> machineRepairs = Enumerable.Empty<MachineRepair>();
            if (string.IsNullOrEmpty(shift))
            {
                machineRepairs = await _repository.GetByAsync(m => m.MachineId == machineId);
            }
            else
            {
                machineRepairs = await _repository.GetByAsync(m => m.MachineId == machineId && m.Shift == shift);
            }

            return _mapper.Map<IEnumerable<MachineRepair>, IEnumerable<MachineRepairResponse>>(machineRepairs);
        }

        public async Task<IEnumerable<MachineRepairResponse>> GetByMonthAsync(string shift, string dateTime)
        {
            if(DateTime.TryParse(dateTime, out DateTime date))
            {
                date = DateTime.Parse(dateTime);

                IEnumerable<MachineRepair> machineRepairs = Enumerable.Empty<MachineRepair>();
                if (string.IsNullOrEmpty(shift))
                {
                    machineRepairs = await _repository.GetByAsync(m => m.Date.Month == date.Month);
                }
                else
                {
                    machineRepairs = await _repository.GetByAsync(m => m.Date.Month == date.Month && m.Shift == shift);
                }

                return _mapper.Map<IEnumerable<MachineRepair>, IEnumerable<MachineRepairResponse>>(machineRepairs);
            }
            else
            {
                throw new InvalidOperationException("Input was not in DateTime format");
            }
        }

        public async Task<IEnumerable<MachineRepairResponse>> GetByMachineIdAndDateAsync(int machineId, string shift, string date)
        {
            if (DateTime.TryParse(date, out DateTime dateTime))
            {
                dateTime = DateTime.Parse(date);
                IEnumerable<MachineRepair> machineRepairs = Enumerable.Empty<MachineRepair>();
                if (string.IsNullOrEmpty(shift))
                {
                    machineRepairs = await _repository.GetByAsync(m => m.MachineId == machineId && m.Date.Month == dateTime.Month);
                }
                else
                {
                    machineRepairs = await _repository.GetByAsync(m => m.MachineId == machineId && m.Date.Month == dateTime.Month && m.Shift == shift);
                }

                return _mapper.Map<IEnumerable<MachineRepair>, IEnumerable<MachineRepairResponse>>(machineRepairs);
            }
            else
            {
                throw new InvalidOperationException("Input was not in DateTime format");
            }
        }

        public async Task<MachineRepairResponse> UpdateAsync(int id, MachineRepairUpdate machineRepairUpdate)
        {
            var machineRepair = await _repository.FindAsync(p => p.Id == id);

            if (machineRepair == null)
                throw new NotFoundException("Not found");

            _repository.Update(machineRepair);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<MachineRepair, MachineRepairResponse>(machineRepair);
        }
    }
}
