using SMT.Domain;
using SMT.ViewModel.Dto.PlanDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanResponse>> GetAllAsync(string shift);

        Task<PlanResponse> GetAsync(int id);

        Task<IEnumerable<PlanResponse>> GetByLineId(int lineId, string shift);

        Task<IEnumerable<PlanResponse>> GetByModelId(int modelId, string shift);

        Task<IEnumerable<PlanResponse>> GetByDate(DateTime date, string shift);

        Task<IEnumerable<PlanResponse>> GetByLineAndDate(int lineId, string shift, DateTime from, DateTime to);

        Task<PlanResponse> AddAsync(PlanCreate planCreate);

        Task<PlanResponse> UpdateAsync(int id, PlanUpdate planUpdate);

        Task<PlanResponse> DeleteAsync(int id);
    }
}
