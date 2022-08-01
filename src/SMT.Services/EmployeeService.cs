using AutoMapper;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.EmployeeDto;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMT.Access.Repository.Interfaces;

namespace SMT.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeResponse> AddAsync(EmployeeCreate employeeCreate)
        {
            var employee = _mapper.Map<EmployeeCreate, Employee>(employeeCreate);

            await _repository.AddAsync(employee);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Employee, EmployeeResponse>(employee);
        }

        public async Task<EmployeeResponse> DeactivateAsync(int id, bool isActive)
        {
            var employee = await _repository.FindAsync(p => p.Id == id);

            if (employee == null)
                throw new NotFoundException("Not found");

            employee.IsActive = isActive;

            _repository.Update(employee);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Employee, EmployeeResponse>(employee);
        }

        public async Task<EmployeeResponse> DeleteAsync(int id)
        {
            var employee = await _repository.FindAsync(p => p.Id == id);

            if (employee == null)
                throw new NotFoundException("Not found");

            _repository.Delete(employee);
            await _unitOfWork.SaveAsync();

            // seems it's not necessary to return deleted employee image
            //var imgUrl = _imageService.LoadUrl(employee.ImagePath);
            //employee.ImagePath = imgUrl;

            return _mapper.Map<Employee, EmployeeResponse>(employee);
        }

        public async Task<IEnumerable<EmployeeResponse>> GetByStatusAsync(bool isActive)
        {
            var employees = await _repository.GetByAsync(e => e.IsActive == isActive);

            return _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(employees);
        }

        public async Task<IEnumerable<EmployeeResponse>> GetAllAsync()
        {
            var employees = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(employees);
        }

        public async Task<EmployeeResponse> GetAsync(int id)
        {
            var employee = await _repository.FindAsync(p => p.Id == id);

            if (employee == null)
                throw new NotFoundException("Not found");

            return _mapper.Map<Employee, EmployeeResponse>(employee);
        }

        public async Task<IEnumerable<EmployeeResponse>> GetByDepartmentAsync(string departmentId, bool isActive)
        {
            var employees = await _repository.GetByDepartmentAsync(departmentId, isActive);

            return _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(employees);
        }

        public async Task<EmployeeResponse> UpdateAsync(int id, EmployeeUpdate employeeUpdate)
        {
            var employee = await _repository.FindAsync(p => p.Id == id);

            if (employee == null)
                throw new NotFoundException("Not found");

            employee = _mapper.Map<EmployeeUpdate, Employee>(employeeUpdate);

            _repository.Update(employee);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Employee, EmployeeResponse>(employee);
        }
    }
}