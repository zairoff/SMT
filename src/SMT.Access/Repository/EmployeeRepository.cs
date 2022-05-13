using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(AppDbContext context) : base(context)
        {
        }

        public async override Task<Employee> FindAsync(Expression<Func<Employee, bool>> expression)
        {
            return await DbSet.Include(e => e.Department).Where(expression).FirstOrDefaultAsync();
        }

        public async override Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await DbSet.Include(e => e.Department).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByAsync(Expression<Func<Employee, bool>> expression)
        {
            return await DbSet.Where(expression).Include(e => e.Department).ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetByDepartmentAsync(string departmentId)
        {
            // Query method but preffered LINQ
            /*var employees = await _context.Departments.Join(_context.Employees,
                department => department.Id,
                employee => employee.DepartmentId,
                (department, employee) => new Employee
                {
                    Id = employee.Id,
                    DepartmentId = department.Id,
                    Department = department,
                    Address = employee.Address,
                    Birthday = employee.Birthday,
                    DepartmentName = employee.DepartmentName,
                    Details = employee.Details,
                    FullName = employee.FullName,
                    ImagePath = employee.ImagePath,
                    Passport = employee.Passport,
                    Phone = employee.Phone,
                    Position = employee.Position
                }).Where(d => d.Department.HierarchyId.IsDescendantOf(HierarchyId.Parse(departmentId))).ToListAsync();*/

            var query = from employee in _context.Employees
                        join department in _context.Departments
                        on employee.DepartmentId equals department.Id
                        where department.HierarchyId.IsDescendantOf(HierarchyId.Parse(departmentId))
                        select employee;

            var employees = await query.Include(e => e.Department).ToListAsync();

            return employees;
        }
    }
}
