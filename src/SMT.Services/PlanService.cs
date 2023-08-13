using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.PlanDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class PlanService : IPlanService
    {
        private readonly IPlanRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IMapper mapper, IUnitOfWork unitOfWork, IPlanRepository repository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public async Task<PlanResponse> AddAsync(PlanCreate planCreate)
        {
            var plan = _mapper.Map<PlanCreate, Plan>(planCreate);

            await _repository.AddAsync(plan);
            await _unitOfWork.SaveAsync();

            plan = await _repository.FindAsync(m => m.Id == plan.Id);

            return _mapper.Map<Plan, PlanResponse>(plan);
        }

        public async Task<PlanResponse> DeleteAsync(int id)
        {
            var plan = await _repository.FindAsync(p => p.Id == id);

            if (plan == null)
                throw new NotFoundException("Plan not found");

            _repository.Delete(plan);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Plan, PlanResponse>(plan);
        }

        public async Task<IEnumerable<PlanResponse>> GetAllAsync(string shift)
        {
            IEnumerable<Plan> plans = Enumerable.Empty<Plan>();

            if (string.IsNullOrEmpty(shift))
            {
                plans = await _repository.GetAllAsync();
            }
            else
            {
                plans = await _repository.GetByAsync(x => x.Shift ==  shift);
            }

            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanResponse>>(plans);
        }

        public async Task<PlanResponse> GetAsync(int id)
        {
            var plan = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<Plan, PlanResponse>(plan);
        }

        public async Task<IEnumerable<PlanResponse>> GetByDate(DateTime date, string shift)
        {
            IEnumerable<Plan> plans = Enumerable.Empty<Plan>();
            if (string.IsNullOrEmpty(shift))
            {
                plans = await _repository.GetByAsync(p => p.Date.Date == date.Date);
            }
            else
            {
                plans = await _repository.GetByAsync(p => p.Date.Date == date.Date && p.Shift == shift);
            }

            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanResponse>>(plans);
        }

        public async Task<IEnumerable<PlanResponse>> GetByLineId(int lineId, string shift)
        {
            IEnumerable<Plan> plans = Enumerable.Empty<Plan>();
            if (string.IsNullOrEmpty(shift))
            {
                plans = await _repository.GetByAsync(p => p.LineId == lineId);
            }
            else
            {
                plans = await _repository.GetByAsync(p => p.LineId == lineId && p.Shift == shift);
            }

            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanResponse>>(plans);
        }

        public async Task<IEnumerable<PlanResponse>> GetByModelId(int modelId, string shift)
        {
            IEnumerable<Plan> plans = Enumerable.Empty<Plan>();
            if (string.IsNullOrEmpty(shift))
            {
                plans = await _repository.GetByAsync(p => p.ModelId == modelId);
            }
            else
            {
                plans = await _repository.GetByAsync(p => p.ModelId == modelId && p.Shift == shift);
            }

            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanResponse>>(plans);
        }

        public async Task<PlanResponse> UpdateAsync(int id, PlanUpdate planUpdate)
        {
            var plan = await _repository.FindAsync(p => p.Id == id);


            if (plan == null)
                throw new NotFoundException("Not found");

            plan.LineId = planUpdate.LineId;
            plan.ModelId = planUpdate.ModelId;
            plan.ProducedCount = planUpdate.ProducedCount;
            plan.RequiredCount = planUpdate.RequiredCount;
            plan.Date = planUpdate.Date;
            plan.Shift = planUpdate.Shift;

            _repository.Update(plan);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Plan, PlanResponse>(plan);
        }

        public async Task<IEnumerable<PlanResponse>> GetByLineAndDate(int lineId, string shift, DateTime from, DateTime to)
        {
            IEnumerable<Plan> plans = Enumerable.Empty<Plan>();
            if (string.IsNullOrEmpty(shift))
            {
                plans = await _repository.GetByAsync(p => p.LineId == lineId && p.Date >= from && p.Date <= to);
            }
            else
            {
                plans = await _repository.GetByAsync(p => p.LineId == lineId && p.Date >= from && p.Date <= to && p.Shift == shift);
            }

            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanResponse>>(plans);
        }
    }
}
