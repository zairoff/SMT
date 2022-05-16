using SMT.ViewModel.Dto.RepairDto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace SMT.Services.Interfaces
{
    public interface IRepairService
    {
        Task<IEnumerable<RepairResponse>> GetAllAsync();

        Task<RepairResponse> GetAsync(int id);

        Task<IEnumerable<RepairResponse>> GetByDateAsync(DateTime date);

        Task<RepairResponse> AddAsync(RepairCreate repairCreate);

        Task<RepairResponse> DeleteAsync(int id);
    }
}
