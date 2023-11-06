using SMT.ViewModel.Dto.ReadyProductDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IReadyProductService
    {
        Task<IEnumerable<ReadyProductResponse>> GetAllAsync();

        Task<ReadyProductResponse> GetAsync(int id);

        Task<IEnumerable<ReadyProductResponse>> GetByEnterDateAsync(DateTime date);

        Task<IEnumerable<ReadyProductResponse>> GetByExitDateAsync(DateTime date);

        Task<IEnumerable<ReadyProductResponse>> GetByEnterDateRangeAsync(DateTime from, DateTime to);

        Task<IEnumerable<ReadyProductResponse>> GetByExitDateRangeAsync(DateTime from, DateTime to);

        Task<ReadyProductResponse> AddAsync(ReadyProductCreate readyProductCreate);

        Task<ReadyProductResponse> UpdateAsync(int id, ReadyProductUpdate readyProductUpdate);

        Task<ReadyProductResponse> DeleteAsync(int id);
    }
}
