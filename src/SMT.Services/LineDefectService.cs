using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.LineDefectDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class LineDefectService : ILineDefectService
    {
        private readonly ILineDefectRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LineDefectService(ILineDefectRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LineDefectResponse> AddAsync(LineDefectCreate lineDefectCreate)
        {
            var lineDefect = await _repository.FindAsync(
                            p => p.LineId == lineDefectCreate.LineId &&
                            p.DefectId == lineDefectCreate.DefectId);

            if (lineDefect != null)
                throw new ConflictException($"{lineDefect.Defect.Name} under {lineDefect.Line.Name} already exist");

            lineDefect = _mapper.Map<LineDefectCreate, LineDefect>(lineDefectCreate);

            await _repository.AddAsync(lineDefect);
            await _unitOfWork.SaveAsync();

            var id = lineDefect.Id;
            lineDefect = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<LineDefect, LineDefectResponse>(lineDefect);
        }

        public async Task<LineDefectResponse> DeleteAsync(int id)
        {
            var lineDefect = await _repository.FindAsync(p => p.Id == id);

            if (lineDefect == null)
                throw new NotFoundException("Not found");

            _repository.Delete(lineDefect);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<LineDefect, LineDefectResponse>(lineDefect);
        }

        public async Task<IEnumerable<LineDefectResponse>> GetAllAsync()
        {
            var lineDefects = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<LineDefect>, IEnumerable<LineDefectResponse>>(lineDefects);
        }

        public async Task<LineDefectResponse> GetAsync(int id)
        {
            var lineDefect = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<LineDefect, LineDefectResponse>(lineDefect);
        }

        public async Task<LineDefectResponse> GetByLineAndDefectIdAsync(int lineId, int defectId)
        {
            var lineDefect = await _repository.FindAsync(p => p.LineId == lineId && p.DefectId == defectId);

            return _mapper.Map<LineDefect, LineDefectResponse>(lineDefect);
        }

        public async Task<IEnumerable<LineDefectResponse>> GetByLineIdAsync(int lineId)
        {
            var lineDefects = await _repository.GetByAsync(p => p.LineId == lineId);

            return _mapper.Map<IEnumerable<LineDefect>, IEnumerable<LineDefectResponse>>(lineDefects);
        }

        public async Task<LineDefectResponse> UpdateAsync(int id, LineDefectUpdate lineDefectUpdate)
        {
            var lineDefect = await _repository.FindAsync(p => p.Id == id);

            if (lineDefect == null)
                throw new NotFoundException("Not found");

            lineDefect.LineId = lineDefectUpdate.LineId;
            lineDefect.DefectId = lineDefectUpdate.DefectId;

            _repository.Update(lineDefect);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<LineDefect, LineDefectResponse>(lineDefect);
        }
    }
}
