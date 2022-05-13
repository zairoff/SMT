using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.RepairerDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class RepairerService : IRepairerService
    {
        private readonly IRepairerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RepairerService(IRepairerRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RepairerResponse> AddAsync(RepairerCreate repairerCreate)
        {
            var repairer = await _repository.FindAsync(r => r.EmployeeId == repairerCreate.EmployeeId);

            if (repairer != null)
                throw new ConflictException($"{repairer.Employee.FullName} already exists");

            repairer = _mapper.Map<RepairerCreate, Repairer>(repairerCreate);

            await _repository.AddAsync(repairer);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Repairer, RepairerResponse>(repairer);
        }

        public async Task<RepairerResponse> DeleteAsync(int id)
        {
            var repairer = await _repository.FindAsync(p => p.Id == id);

            if (repairer == null)
                throw new NotFoundException("Not found");

            _repository.Delete(repairer);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Repairer, RepairerResponse>(repairer);
        }

        public async Task<IEnumerable<RepairerResponse>> GetAllAsync()
        {
            var repairers = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<Repairer>, IEnumerable<RepairerResponse>>(repairers);
        }

        public async Task<RepairerResponse> GetAsync(int id)
        {
            var repairer = await _repository.FindAsync(p => p.Id == id);

            if (repairer == null)
                throw new NotFoundException("Not found");

            return _mapper.Map<Repairer, RepairerResponse>(repairer);
        }
    }
}
