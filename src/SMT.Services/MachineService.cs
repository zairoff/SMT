using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.MachineDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class MachineService : IMachineService
    {
        private readonly IMachineRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MachineService(IMapper mapper, IUnitOfWork unitOfWork, IMachineRepository repository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<MachineResponse> AddAsync(MachineCreate machineCreate)
        {
            var machine = await _repository.FindAsync(p => p.Name == machineCreate.Name);

            if (machine != null)
                throw new ConflictException($"{machineCreate.Name} already exists");

            machine = _mapper.Map<MachineCreate, Machine>(machineCreate);

            await _repository.AddAsync(machine);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Machine, MachineResponse>(machine);
        }

        public async Task<MachineResponse> DeleteAsync(int id)
        {
            var machine = await _repository.FindAsync(p => p.Id == id);

            if (machine == null)
                throw new NotFoundException("Not found");

            _repository.Delete(machine);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Machine, MachineResponse>(machine);
        }

        public async Task<IEnumerable<MachineResponse>> GetAllAsync()
        {
            var machines = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<Machine>, IEnumerable<MachineResponse>>(machines);
        }

        public async Task<MachineResponse> GetAsync(int id)
        {
            var machine = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<Machine, MachineResponse>(machine);
        }
    }
}
