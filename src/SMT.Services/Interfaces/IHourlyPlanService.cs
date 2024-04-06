using SMT.ViewModel.Dto.HourlyPlanDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IHourlyPlanService
    {
        Task NotifyAsync();

        Task<IEnumerable<HourlyPlanResponse>> GetAllAsync();

        Task<HourlyPlanResponse> GetAsync(int id);

        Task<IEnumerable<HourlyPlanResponse>> GetByLineId(int lineId);

        Task<IEnumerable<HourlyPlanResponse>> GetByProductId(int productId);

        Task<IEnumerable<HourlyPlanResponse>> GetByBrandId(int brandId);

        Task<IEnumerable<HourlyPlanResponse>> GetByModelId(int modelId);

        Task<IEnumerable<HourlyPlanResponse>> GetByDate(DateTime date);

        Task<IEnumerable<HourlyPlanResponse>> GetByLineAndDate(int lineId, DateTime from, DateTime to);

        Task<HourlyPlanResponse> AddAsync(HourlyPlanCreate planCreate);

        Task<HourlyPlanResponse> UpdateAsync(int id, HourlyPlanUpdate planUpdate);

        Task<HourlyPlanResponse> DeleteAsync(int id);
    }
}
