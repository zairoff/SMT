using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.RepairDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class RepairService : IRepairService
    {
        private readonly IRepairRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RepairService(IRepairRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RepairResponse> AddAsync(RepairCreate repairCreate)
        {
            var repair = _mapper.Map<RepairCreate, Repair>(repairCreate);

            await _repository.AddAsync(repair);
            await _unitOfWork.SaveAsync();

            repair = await _repository.FindAsync(r => r.Id == repair.Id);

            return _mapper.Map<Repair, RepairResponse>(repair);
        }

        public async Task<RepairResponse> DeleteAsync(int id)
        {
            var repair = await _repository.FindAsync(p => p.Id == id);

            if (repair == null)
                throw new NotFoundException("Not found");

            _repository.Delete(repair);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Repair, RepairResponse>(repair);
        }

        public async Task<IEnumerable<RepairResponse>> GetAllAsync()
        {
            var repairs = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<Repair>, IEnumerable<RepairResponse>>(repairs);
        }

        public async Task<RepairResponse> GetAsync(int id)
        {
            var repair = await _repository.FindAsync(p => p.Id == id);

            if (repair == null)
                throw new NotFoundException("Not found");

            return _mapper.Map<Repair, RepairResponse>(repair);
        }

        public async Task<IEnumerable<RepairResponse>> GetByDateAsync(DateTime date)
        {
            var repairs = await _repository.GetByAsync(r => r.Date.Date == date);

            return _mapper.Map<IEnumerable<Repair>, IEnumerable<RepairResponse>>(repairs);
        }
    }
}
