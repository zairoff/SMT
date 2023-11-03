using SMT.ViewModel.Dto.DepartmentDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentResponse>> GetAllAsync(bool? isActive);

        Task<DepartmentResponse> GetAsync(int id);

        Task<DepartmentResponse> GetByNameAsync(string name);

        Task<IEnumerable<DepartmentResponse>> GetByHierarchyId(string hierarchyId);

        Task<DepartmentResponse> AddAsync(DepartmentCreate departmentCreate);

        Task<DepartmentResponse> UpdateAsync(int id, DepartmentUpdate departmentUpdate);

        Task<DepartmentResponse> DeleteAsync(int id);
    }
}
