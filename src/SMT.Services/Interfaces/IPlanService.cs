using SMT.ViewModel.Dto.PlanDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanResponse>> GetAllAsync();

        Task<PlanResponse> GetAsync(int id);

        Task<IEnumerable<PlanResponse>> GetByLineId(int lineId);

        Task<IEnumerable<PlanResponse>> GetByModelId(int modelId);

        Task<IEnumerable<PlanResponse>> GetByDate(DateTime date);

        Task<IEnumerable<PlanResponse>> GetByLineAndDate(int lineId, DateTime from, DateTime to);

        Task<PlanResponse> AddAsync(PlanCreate planCreate);

        Task<PlanResponse> UpdateAsync(int id, PlanUpdate planUpdate);

        Task<PlanResponse> DeleteAsync(int id);
    }
}
