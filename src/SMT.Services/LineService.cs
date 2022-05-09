using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.LineDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class LineService : ILineService
    {
        private readonly ILineRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LineService(ILineRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LineResponse> AddAsync(LineCreate lineCreate)
        {
            var line = await _repository.FindAsync(p => p.Name == lineCreate.Name);

            if (line != null)
                throw new ConflictException($"Line {lineCreate.Name} already exists");

            line = _mapper.Map<LineCreate, Line>(lineCreate);

            await _repository.AddAsync(line);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Line, LineResponse>(line);
        }

        public async Task<LineResponse> DeleteAsync(int id)
        {
            var line = await _repository.FindAsync(p => p.Id == id);

            if (line == null)
                throw new NotFoundException("Not found");

            _repository.Delete(line);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Line, LineResponse>(line);
        }

        public async Task<IEnumerable<LineResponse>> GetAllAsync()
        {
            var lines = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<Line>, IEnumerable<LineResponse>>(lines);
        }

        public async Task<LineResponse> GetAsync(int id)
        {
            var line = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<Line, LineResponse>(line);
        }

        public async Task<LineResponse> GetByNameAsync(string name)
        {
            var line = await _repository.FindAsync(p => p.Name == name);

            return _mapper.Map<Line, LineResponse>(line);
        }

        public async Task<LineResponse> UpdateAsync(int id, LineUpdate lineUpdate)
        {
            var line = await _repository.FindAsync(p => p.Id == id);

            if (line == null)
                throw new NotFoundException("Not found");

            line.Name = lineUpdate.Name;

            _repository.Update(line);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<Line, LineResponse>(line);
        }
    }
}
