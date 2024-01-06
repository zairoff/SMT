using SMT.ViewModel.Dto.PlanActivityDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IPlanActivityService
    {
        Task<IEnumerable<PlanActivityResponse>> GetAllAsync();

        Task<PlanActivityResponse> GetAsync(int id);

        Task<IEnumerable<PlanActivityResponse>> GetByLineId(int lineId);

        Task<IEnumerable<PlanActivityResponse>> GetByDate(DateTime date);

        Task<IEnumerable<PlanActivityResponse>> GetByDateRange(DateTime from, DateTime to);

        Task<IEnumerable<PlanActivityResponse>> GetByLineAndDate(int lineId, DateTime date);

        Task<PlanActivityResponse> AddAsync(PlanActivityCreate planActivityCreate);

        Task<PlanActivityResponse> UpdateAsync(int id, PlanActivityUpdate planActivityUpdate);

        Task<PlanActivityResponse> DeleteAsync(int id);
    }
}
