using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.Services.Interfaces.FileSystem;
using SMT.ViewModel.Dto.EmployeeDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository repository, IUnitOfWork unitOfWork, IMapper mapper,
                                IImageService imageService)
        {
            _imageService = imageService;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<EmployeeResponse> AddAsync(EmployeeCreate employeeCreate)
        {
            var employee = await _repository.FindAsync(p => p.Passport == employeeCreate.Passport);

            if (employee != null)
                throw new ConflictException($"{employeeCreate.Passport} already exists");

            employee = _mapper.Map<EmployeeCreate, Employee>(employeeCreate);

            var fileName = await _imageService.SaveAsync(employeeCreate.File);
            employee.ImagePath = fileName;

            await _repository.AddAsync(employee);
            await _unitOfWork.SaveAsync();

            UpdateEmployeeImageUrl(employee);

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

        public async Task<IEnumerable<EmployeeResponse>> GetAllAsync()
        {
            var employees = await _repository.GetAllAsync();

            UpdateEmployeesImageUrl(employees);

            return _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(employees);
        }

        public async Task<EmployeeResponse> GetAsync(int id)
        {
            var employee = await _repository.FindAsync(p => p.Id == id);

            if (employee == null)
                throw new NotFoundException("Not found");

            UpdateEmployeeImageUrl(employee);

            return _mapper.Map<Employee, EmployeeResponse>(employee);
        }

        public async Task<IEnumerable<EmployeeResponse>> GetByDepartmentAsync(string departmentId)
        {
            var employees = await _repository.GetByDepartmentAsync(departmentId);
            UpdateEmployeesImageUrl(employees);

            return _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeResponse>>(employees);
        }

        public async Task<EmployeeResponse> UpdateAsync(int id, EmployeeUpdate employeeUpdate)
        {
            var employee = await _repository.FindAsync(p => p.Id == id);

            if (employee == null)
                throw new NotFoundException("Not found");

            employee = await _repository.FindAsync(p => p.Passport == employeeUpdate.Passport);

            if (employee != null)
                throw new ConflictException($"{employeeUpdate.Passport} already exists");

            employee = _mapper.Map<EmployeeUpdate, Employee>(employeeUpdate);

            var fileName = await _imageService.SaveAsync(employeeUpdate.File);
            var imagePath = _imageService.LoadUrl(fileName);

            employee.ImagePath = imagePath;

            _repository.Update(employee);
            await _unitOfWork.SaveAsync();

            UpdateEmployeeImageUrl(employee);

            return _mapper.Map<Employee, EmployeeResponse>(employee);
        }

        private Employee UpdateEmployeeImageUrl(Employee employee)
        {
            var url = _imageService.LoadUrl(employee.ImagePath);
            employee.ImagePath = url;
            return employee;
        }

        private IEnumerable<Employee> UpdateEmployeesImageUrl(IEnumerable<Employee> employees)
        {
            foreach(var employee in employees)
            {
                var url = _imageService.LoadUrl(employee.ImagePath);
                employee.ImagePath = url;
            }

            return employees;
        }
    }
}
