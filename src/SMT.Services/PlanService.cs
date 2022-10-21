using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.EmployeeDto;
using SMT.ViewModel.Dto.PlanDto;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<PlanResponse>> GetAllAsync()
        {
            var plans = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanResponse>>(plans);
        }

        public async Task<PlanResponse> GetAsync(int id)
        {
            var plan = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<Plan, PlanResponse>(plan);
        }

        public async Task<IEnumerable<PlanResponse>> GetByDate(DateTime date)
        {
            var plans = await _repository.GetByAsync(p => p.Date.Date == date.Date);

            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanResponse>>(plans);
        }

        public async Task<IEnumerable<PlanResponse>> GetByLineId(int lineId)
        {
            var plans = await _repository.GetByAsync(p => p.LineId == lineId);

            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanResponse>>(plans);
        }

        public async Task<IEnumerable<PlanResponse>> GetByModelId(int modelId)
        {
            var plans = await _repository.GetByAsync(p => p.ModelId == modelId);

            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanResponse>>(plans);
        }

        public async Task<IEnumerable<PlanResponse>> GetByBrandId(int brandId)
        {
            var plans = await _repository.GetByAsync(p => p.Model.ProductBrand.BrandId == brandId);

            return _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanResponse>>(plans);
        }

        public async Task<IEnumerable<PlanResponse>> GetByProductId(int productId)
        {
            var plans = await _repository.GetByAsync(p => p.Model.ProductBrand.ProductId == productId);

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

            _repository.Update(plan);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Plan, PlanResponse>(plan);
        }
    }
}
