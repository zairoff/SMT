using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.PlanActivityDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class PlanActivityService : IPlanActivityService
    {
        private readonly IPlanActivityRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanActivityService(IUnitOfWork unitOfWork, IPlanActivityRepository repository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PlanActivityResponse> AddAsync(PlanActivityCreate planActivityCreate)
        {
            var planActivity = _mapper.Map<PlanActivityCreate, PlanActivity>(planActivityCreate);

            await _repository.AddAsync(planActivity);
            await _unitOfWork.SaveAsync();

            planActivity = await _repository.FindAsync(m => m.Id == planActivity.Id);

            return _mapper.Map<PlanActivity, PlanActivityResponse>(planActivity);
        }

        public async Task<PlanActivityResponse> DeleteAsync(int id)
        {
            var plan = await _repository.FindAsync(p => p.Id == id);

            if (plan == null)
                throw new NotFoundException("PlanActivity not found");

            _repository.Delete(plan);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<PlanActivity, PlanActivityResponse>(plan);
        }

        public async Task<IEnumerable<PlanActivityResponse>> GetAllAsync()
        {
            var plans = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<PlanActivity>, IEnumerable<PlanActivityResponse>>(plans);
        }

        public async Task<PlanActivityResponse> GetAsync(int id)
        {
            var plan = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<PlanActivity, PlanActivityResponse>(plan);
        }

        public async Task<IEnumerable<PlanActivityResponse>> GetByDate(DateTime date)
        {
            var plans = await _repository.GetByAsync(p => p.Date.Date == date.Date);

            return _mapper.Map<IEnumerable<PlanActivity>, IEnumerable<PlanActivityResponse>>(plans);
        }

        public async Task<IEnumerable<PlanActivityResponse>> GetByLineAndDate(int lineId, DateTime date)
        {
            var plans = await _repository.GetByAsync(p => p.Date.Date == date.Date && p.LineId == lineId);

            return _mapper.Map<IEnumerable<PlanActivity>, IEnumerable<PlanActivityResponse>>(plans);
        }

        public async Task<IEnumerable<PlanActivityResponse>> GetByLineId(int lineId)
        {
            var plans = await _repository.GetByAsync(p => p.LineId == lineId);

            return _mapper.Map<IEnumerable<PlanActivity>, IEnumerable<PlanActivityResponse>>(plans);
        }

        public async Task<PlanActivityResponse> UpdateAsync(int id, PlanActivityUpdate planActivityUpdate)
        {
            var plan = await _repository.FindAsync(p => p.Id == id);


            if (plan == null)
                throw new NotFoundException("Not found");

            plan.LineId = planActivityUpdate.LineId;
            plan.Expires = planActivityUpdate.Expires;
            plan.Status = planActivityUpdate.Status;
            plan.Date = planActivityUpdate.Date;
            plan.Act = planActivityUpdate.Act;
            plan.Reason = planActivityUpdate.Reason;
            plan.Responsible = planActivityUpdate.Responsible;

            _repository.Update(plan);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<PlanActivity, PlanActivityResponse>(plan);
        }
    }
}
