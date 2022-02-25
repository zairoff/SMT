using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.ViewModel.Dto.DefectDto;
using SMT.Domain;
using SMT.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using SMT.ViewModel.Exceptions;

namespace SMT.Services
{
    public class DefectService : IDefectService
    {
        private readonly IDefectRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DefectService(IDefectRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<DefectResponse> AddAsync(DefectCreate defectCreate)
        {
            var defect = await _repository.FindAsync(p => p.Name == defectCreate.Name);

            if (defect != null)
                throw new ConflictException();

            defect = _mapper.Map<DefectCreate, Defect>(defectCreate);

            await _repository.AddAsync(defect);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Defect, DefectResponse>(defect);
        }

        public async Task<DefectResponse> DeleteAsync(int id)
        {
            var defect = await _repository.FindAsync(p => p.Id == id);

            if (defect == null)
                throw new NotFoundException();

            _repository.Delete(defect);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Defect, DefectResponse>(defect);
        }

        public async Task<IEnumerable<DefectResponse>> GetAllAsync()
        {
            var defect = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<Defect>, IEnumerable<DefectResponse>>(defect);
        }

        public async Task<DefectResponse> GetAsync(int id)
        {
            var defect = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<Defect, DefectResponse>(defect);
        }

        public async Task<DefectResponse> GetByNameAsync(string name)
        {
            var defect = await _repository.FindAsync(p => p.Name == name);

            return _mapper.Map<Defect, DefectResponse>(defect);
        }

        public async Task<DefectResponse> UpdateAsync(int id, DefectUpdate defectUpdate)
        {
            var defect = await _repository.FindAsync(p => p.Id == id);

            if (defect == null)
                throw new NotFoundException();

            defect.Name = defectUpdate.Name;

            _repository.Update(defect);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Defect, DefectResponse>(defect);
        }
    }
}
