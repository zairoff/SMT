using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.InstructionPositionDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class InstructionPositionService : IInstructionPositionService
    {
        private readonly IInstructionPositionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InstructionPositionService(IInstructionPositionRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<InstructionPositionResponse> AddAsync(InstructionPositionCreate positionCreate)
        {
            var position = _mapper.Map<InstructionPositionCreate, InstructionPosition>(positionCreate);

            await _repository.AddAsync(position);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<InstructionPosition, InstructionPositionResponse>(await _repository.FindAsync(p => p.Id == position.Id));
        }

        public async Task<InstructionPositionResponse> DeleteAsync(int id)
        {
            var position = await _repository.FindAsync(p => p.Id == id);

            if (position == null)
                throw new NotFoundException("Not found");

            _repository.Delete(position);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<InstructionPosition, InstructionPositionResponse>(position);
        }

        public async Task<IEnumerable<InstructionPositionResponse>> GetAllAsync()
        {
            var positions = await _repository.GetAllAsync();

            return _mapper.Map<IEnumerable<InstructionPosition>, IEnumerable<InstructionPositionResponse>>(positions);
        }

        public async Task<InstructionPositionResponse> GetAsync(int id)
        {
            var position = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<InstructionPosition, InstructionPositionResponse>(position);
        }

        public async Task<IEnumerable<InstructionPositionResponse>> GetByLineAsync(int lineId)
        {
            var positions = await _repository.GetByLineAsync(lineId);

            return _mapper.Map<IEnumerable<InstructionPosition>, IEnumerable<InstructionPositionResponse>>(positions);
        }

        public async Task<InstructionPositionResponse> UpdateAsync(int id, InstructionPositionUpdate positionUpdate)
        {
            var position = await _repository.FindAsync(p => p.Id == id);

            if (position == null)
                throw new NotFoundException("Not found");

            position.LineId = positionUpdate.LineId;
            position.Name = positionUpdate.Name;
            position.Order = positionUpdate.Order;

            _repository.Update(position);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<InstructionPosition, InstructionPositionResponse>(position);
        }
    }
}
