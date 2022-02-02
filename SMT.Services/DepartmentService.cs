using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.Access;
using SMT.Common.Dto.DepartmentDto;
using SMT.Common.Exceptions;
using SMT.Domain;
using SMT.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository<Department> _repository;
        private readonly IMapper _mapper;

        public DepartmentService(IRepository<Department> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DepartmentResponse> AddAsync(DepartmentCreate departmentCreate)
        {
            var department = await _repository.Get()
                                                .Where(d => d.Name == departmentCreate.Name)
                                                .FirstOrDefaultAsync();

            if (department != null)
                throw new ConflictException();

            department = _mapper.Map<DepartmentCreate, Department>(departmentCreate);

            await _repository.AddAsync(department);

            return _mapper.Map<DepartmentResponse>(department);
        }

        public async Task<DepartmentResponse> DeleteAsync(int id)
        {
            var department = await _repository.Get()
                                                .Where(d => d.Id == id)
                                                .FirstOrDefaultAsync();

            if (department == null)
                throw new NotFoundException();

            await _repository.DeleteAsync(department);

            return _mapper.Map<DepartmentResponse>(department);
        }

        public async Task<IEnumerable<DepartmentResponse>> GetAllAsync()
        {
            var departments = await _repository.GetAll().ToListAsync();

            return _mapper.Map<IEnumerable<DepartmentResponse>>(departments);
        }

        public async Task<DepartmentResponse> GetAsync(int id)
        {
            var department = await _repository.Get()
                                                .Where(d => d.Id == id)
                                                .FirstOrDefaultAsync();

            return _mapper.Map<DepartmentResponse>(department);
        }

        public async Task<DepartmentResponse> GetByNameAsync(string name)
        {
            var department = await _repository.Get()
                                                .Where(d => d.Name == name)
                                                .FirstOrDefaultAsync();

            return _mapper.Map<DepartmentResponse>(department);
        }

        public async Task<DepartmentResponse> UpdateAsync(int id, DepartmentUpdate departmentUpdate)
        {
            var department = await _repository.Get()
                                                .Where(d => d.Id == id)
                                                .FirstOrDefaultAsync();

            if (department == null)
                throw new NotFoundException();

            department.Name = departmentUpdate.Name;

            await _repository.UpdateAsync(department);

            return _mapper.Map<DepartmentResponse>(department);
        }
    }
}
