using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMT.Access;
using SMT.Common.Dto.PcbPositionDto;
using SMT.Common.Exceptions;
using SMT.Domain;
using SMT.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class PcbPositionService : IPcbPositionService
    {
        private readonly IRepository<PcbPosition> _repository;
        private readonly IMapper _mapper;

        public PcbPositionService(IRepository<PcbPosition> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PcbPositionResponse> AddAsync(PcbPositionCreate positionCreate)
        {
            var position = await _repository.Get().Where(p => p.Position == positionCreate.Position).FirstOrDefaultAsync();

            if (position != null)
                throw new ConflictException();

            position = _mapper.Map<PcbPositionCreate, PcbPosition>(positionCreate);

            await _repository.AddAsync(position);

            return _mapper.Map<PcbPosition, PcbPositionResponse>(position);
        }

        public async Task<PcbPositionResponse> DeleteAsync(int id)
        {
            var position = await _repository.Get().Where(p => p.Id == id).FirstOrDefaultAsync();

            if (position == null)
                throw new NotFoundException();

            await _repository.DeleteAsync(position);

            return _mapper.Map<PcbPosition, PcbPositionResponse>(position);
        }

        public async Task<IEnumerable<PcbPositionResponse>> GetAllAsync()
        {
            var positions = await _repository.GetAll().ToListAsync();

            return _mapper.Map<IEnumerable<PcbPosition>, IEnumerable<PcbPositionResponse>>(positions);
        }

        public async Task<PcbPositionResponse> GetAsync(int id)
        {
            var position = await _repository.Get().Where(p => p.Id == id).FirstOrDefaultAsync();

            return _mapper.Map<PcbPosition, PcbPositionResponse>(position);
        }

        public async Task<PcbPositionResponse> GetByNameAsync(string name)
        {
            var position = await _repository.Get().Where(p => p.Position == name).FirstOrDefaultAsync();

            return _mapper.Map<PcbPosition, PcbPositionResponse>(position);
        }

        public async Task<PcbPositionResponse> UpdateAsync(int id, PcbPositionUpdate positionUpdate)
        {
            var position = await _repository.Get().Where(p => p.Id == id).FirstOrDefaultAsync();

            if (position == null)
                throw new NotFoundException();

            position.Position = positionUpdate.Position;

            await _repository.UpdateAsync(position);

            return _mapper.Map<PcbPosition, PcbPositionResponse>(position);
        }
    }
}
