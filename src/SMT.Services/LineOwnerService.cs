using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.LineOwnerDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class LineOwnerService : ILineOwnerService
    {
        private readonly ILineOwnerRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LineOwnerService(ILineOwnerRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LineOwnerResponse> AddAsync(LineOwnerCreate lineOwnerCreate)
        {
            var lineOwner = await _repository.FindAsync(r => r.EmployeeId == lineOwnerCreate.EmployeeId);

            if (lineOwner != null)
                throw new ConflictException($"{lineOwner.Employee.FullName} already exists");

            lineOwner = _mapper.Map<LineOwnerCreate, LineOwner>(lineOwnerCreate);

            await _repository.AddAsync(lineOwner);
            await _unitOfWork.SaveAsync();

            // need to find other way
            lineOwner = await _repository.FindAsync(r => r.Id == lineOwner.Id);

            return _mapper.Map<LineOwner, LineOwnerResponse>(lineOwner);
        }

        public async Task<LineOwnerResponse> DeleteAsync(int id)
        {
            var lineOwner = await _repository.FindAsync(p => p.Id == id);

            if (lineOwner == null)
                throw new NotFoundException("Not found");

            _repository.Delete(lineOwner);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<LineOwner, LineOwnerResponse>(lineOwner);
        }

        public async Task<IEnumerable<LineOwnerResponse>> GetAllAsync()
        {
            var lineOwners = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<LineOwner>, IEnumerable<LineOwnerResponse>>(lineOwners);
        }

        public async Task<LineOwnerResponse> GetAsync(int id)
        {
            var lineOwner = await _repository.FindAsync(p => p.Id == id);

            if (lineOwner == null)
                throw new NotFoundException("Not found");

            return _mapper.Map<LineOwner, LineOwnerResponse>(lineOwner);
        }
    }
}
