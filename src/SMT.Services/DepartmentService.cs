﻿using AutoMapper;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.ViewModel.Dto.DepartmentDto;
using SMT.Domain;
using SMT.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMT.ViewModel.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace SMT.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DepartmentResponse> AddAsync(DepartmentCreate departmentCreate)
        {
            var department = await _repository.FindAsync(d => d.Name == departmentCreate.Name);                                                

            if (department != null)
                throw new ConflictException($"{departmentCreate.Name} already exists");

            department = _mapper.Map<DepartmentCreate, Department>(departmentCreate);

            await _repository.AddAsync(department);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<DepartmentResponse>(department);
        }

        public async Task<DepartmentResponse> DeleteAsync(int id)
        {
            var department = await _repository.FindAsync(d => d.Id == id);

            if (department == null)
                throw new NotFoundException();

            _repository.Delete(department);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<DepartmentResponse>(department);
        }

        public async Task<IEnumerable<DepartmentResponse>> GetAllAsync()
        {
            var departments = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<DepartmentResponse>>(departments);
        }

        public async Task<DepartmentResponse> GetAsync(int id)
        {
            var department = await _repository.FindAsync(d => d.Id == id);

            return _mapper.Map<DepartmentResponse>(department);
        }

        public async Task<IEnumerable<DepartmentResponse>> GetByHierarchyId(string hierarchyId)
        {
            var departments = await _repository.GetByHierarchyIdsync(
                d => d.HierarchyId.IsDescendantOf(HierarchyId.Parse(hierarchyId)));

            return _mapper.Map<IEnumerable<DepartmentResponse>>(departments);
        }

        public async Task<DepartmentResponse> GetByNameAsync(string name)
        {
            var department = await _repository.FindAsync(d => d.Name == name);

            return _mapper.Map<DepartmentResponse>(department);
        }

        public async Task<DepartmentResponse> UpdateAsync(int id, DepartmentUpdate departmentUpdate)
        {
            var department = await _repository.FindAsync(d => d.Id == id);

            if (department == null)
                throw new NotFoundException();

            department.Name = departmentUpdate.Name;

            _repository.Update(department);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<DepartmentResponse>(department);
        }
    }
}
