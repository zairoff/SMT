using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.MachineRepairDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class MachineRepairService : IMachineRepairService
    {
        private readonly IMachineRepairRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MachineRepairService(IMachineRepairRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<MachineRepairResponse> AddAsync(MachineRepairCreate machinerRepairCreate)
        {
            var machineRepair = _mapper.Map<MachineRepairCreate, MachineRepair>(machinerRepairCreate);

            await _repository.AddAsync(machineRepair);
            await _unitOfWork.SaveAsync();

            machineRepair = await _repository.FindAsync(r => r.Id == machineRepair.Id);

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

        public async Task<IEnumerable<MachineRepairResponse>> GetAllAsync()
        {
            var machineRepairs = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<MachineRepair>, IEnumerable<MachineRepairResponse>>(machineRepairs);
        }

        public async Task<MachineRepairResponse> GetAsync(int id)
        {
            var machineRepair = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<MachineRepair, MachineRepairResponse>(machineRepair);
        }
    }
}
