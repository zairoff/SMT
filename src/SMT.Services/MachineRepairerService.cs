using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.MachineRepairerDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class MachineRepairerService : IMachineRepairerService
    {
        private readonly IMachineRepairerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MachineRepairerService(IMachineRepairerRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MachineRepairerResponse> AddAsync(MachineRepairerCreate machineRepairerCreate)
        {
            var repairer = await _repository.FindAsync(r => r.EmployeeId == machineRepairerCreate.EmployeeId);

            if (repairer != null)
                throw new ConflictException($"{repairer.Employee.FullName} already exists");

            repairer = _mapper.Map<MachineRepairerCreate, MachineRepairer>(machineRepairerCreate);

            await _repository.AddAsync(repairer);
            await _unitOfWork.SaveAsync();

            // need to find other way
            repairer = await _repository.FindAsync(r => r.Id == repairer.Id);

            return _mapper.Map<MachineRepairer, MachineRepairerResponse>(repairer);
        }

        public async Task<MachineRepairerResponse> DeleteAsync(int id)
        {
            var repairer = await _repository.FindAsync(p => p.Id == id);

            if (repairer == null)
                throw new NotFoundException("Not found");

            _repository.Delete(repairer);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<MachineRepairer, MachineRepairerResponse>(repairer);
        }

        public async Task<IEnumerable<MachineRepairerResponse>> GetAllAsync()
        {
            var repairers = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<MachineRepairer>, IEnumerable<MachineRepairerResponse>>(repairers);
        }

        public async Task<MachineRepairerResponse> GetAsync(int id)
        {
            var repairer = await _repository.FindAsync(p => p.Id == id);

            if (repairer == null)
                throw new NotFoundException("Not found");

            return _mapper.Map<MachineRepairer, MachineRepairerResponse>(repairer);
        }
    }
}
