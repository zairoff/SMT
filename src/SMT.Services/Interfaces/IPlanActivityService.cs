using SMT.ViewModel.Dto.PlanActivityDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IPlanActivityService
    {
        Task<IEnumerable<PlanActivityResponse>> GetAllAsync(string shift);

        Task<PlanActivityResponse> GetAsync(int id);

        Task<IEnumerable<PlanActivityResponse>> GetByLineId(int lineId, string shift);

        Task<IEnumerable<PlanActivityResponse>> GetByDate(string shift, DateTime date);

        Task<IEnumerable<PlanActivityResponse>> GetByLineAndDate(int lineId, string shift, DateTime date);

        Task<PlanActivityResponse> AddAsync(PlanActivityCreate planActivityCreate);

        Task<PlanActivityResponse> UpdateAsync(int id, PlanActivityUpdate planActivityUpdate);

        Task<PlanActivityResponse> DeleteAsync(int id);
    }
}
