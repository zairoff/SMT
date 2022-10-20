using SMT.ViewModel.Dto.EmployeeDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponse>> GetAllAsync();

        Task<EmployeeResponse> GetAsync(int id);

        Task<EmployeeResponse> AddAsync(PlanCreate employeeCreate);

        Task<EmployeeResponse> UpdateAsync(int id, EmployeeUpdate employeeUpdate);

        Task<EmployeeResponse> DeactivateAsync(int id, bool isActive);

        Task<EmployeeResponse> DeleteAsync(int id);

        Task<IEnumerable<EmployeeResponse>> GetByDepartmentAsync(string departmentId, bool isActive);

        Task<IEnumerable<EmployeeResponse>> GetByStatusAsync(bool isActive);
    }
}
