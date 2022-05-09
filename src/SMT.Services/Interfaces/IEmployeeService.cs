using SMT.ViewModel.Dto.EmployeeDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeResponse>> GetAllAsync();

        Task<EmployeeResponse> GetAsync(int id);

        Task<EmployeeResponse> GetByNameAsync(string name);

        Task<EmployeeResponse> AddAsync(EmployeeCreate employeeCreate);

        Task<EmployeeResponse> UpdateAsync(int id, EmployeeUpdate employeeUpdate);

        Task<EmployeeResponse> DeleteAsync(int id);
    }
}
