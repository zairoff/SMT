using SMT.ViewModel.Dto.EmployeeDto;
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

        Task<IEnumerable<PlanResponse>> GetByProductId(int productId);

        Task<IEnumerable<PlanResponse>> GetByBrandId(int brandId);

        Task<IEnumerable<PlanResponse>> GetByModelId(int modelId);

        Task<IEnumerable<PlanResponse>> GetByDate(DateTime date);

        Task<PlanResponse> AddAsync(PlanCreate planCreate);

        Task<PlanResponse> UpdateAsync(int id, PlanUpdate planUpdate);

        Task<PlanResponse> DeleteAsync(int id);
    }
}
