using SMT.ViewModel.Dto.EmployeeDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponse>> GetAllAsync();

        Task<EmployeeResponse> GetAsync(int id);

        Task<EmployeeResponse> AddAsync(EmployeeCreate employeeCreate);

        Task<EmployeeResponse> UpdateAsync(int id, EmployeeUpdate employeeUpdate);

        Task<EmployeeResponse> DeleteAsync(int id);

        Task<IEnumerable<EmployeeResponse>> GetByDepartmentAsync(string departmentId);
    }
}
