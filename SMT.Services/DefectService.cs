using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.Access;
using SMT.Common.Dto.DefectDto;
using SMT.Common.Exceptions;
using SMT.Domain;
using SMT.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class DefectService : IDefectService
    {
        private readonly IRepository<Defect> _repository;
        private readonly IMapper _mapper;

        public DefectService(IRepository<Defect> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<DefectResponse> AddAsync(DefectCreate defectCreate)
        {
            var defect = await _repository.Get().Where(p => p.Name == defectCreate.Name).FirstOrDefaultAsync();

            if (defect != null)
                throw new ConflictException();

            defect = _mapper.Map<DefectCreate, Defect>(defectCreate);

            await _repository.AddAsync(defect);

            return _mapper.Map<Defect, DefectResponse>(defect);
        }

        public async Task<DefectResponse> DeleteAsync(int id)
        {
            var defect = await _repository.Get().Where(p => p.Id == id).FirstOrDefaultAsync();

            if (defect == null)
                throw new NotFoundException();

            await _repository.DeleteAsync(defect);

            return _mapper.Map<Defect, DefectResponse>(defect);
        }

        public async Task<IEnumerable<DefectResponse>> GetAllAsync()
        {
            var defect = await _repository.GetAll().ToListAsync();

            return _mapper.Map<IEnumerable<Defect>, IEnumerable<DefectResponse>>(defect);
        }

        public async Task<DefectResponse> GetAsync(int id)
        {
            var defect = await _repository.Get().Where(p => p.Id == id).FirstOrDefaultAsync();

            return _mapper.Map<Defect, DefectResponse>(defect);
        }

        public async Task<DefectResponse> GetByNameAsync(string name)
        {
            var defect = await _repository.Get().Where(p => p.Name == name).FirstOrDefaultAsync();

            return _mapper.Map<Defect, DefectResponse>(defect);
        }

        public async Task<DefectResponse> UpdateAsync(int id, DefectUpdate defectUpdate)
        {
            var defect = await _repository.Get().Where(p => p.Id == id).FirstOrDefaultAsync();

            if (defect == null)
                throw new NotFoundException();

            defect.Name = defectUpdate.Name;

            await _repository.UpdateAsync(defect);

            return _mapper.Map<Defect, DefectResponse>(defect);
        }
    }
}
