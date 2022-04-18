using SMT.ViewModel.Dto.DepartmentDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponse>> GetAllAsync();

        Task<DepartmentResponse> GetAsync(int id);

        Task<DepartmentResponse> GetByNameAsync(string name);

        Task<IEnumerable<DepartmentResponse>> GetByHierarchyIdAsync(string hierarchyId);

        Task<DepartmentResponse> AddAsync(DepartmentCreate departmentCreate);

        Task<DepartmentResponse> UpdateAsync(int id, DepartmentUpdate departmentUpdate);

        Task<DepartmentResponse> DeleteAsync(int id);
    }
}
