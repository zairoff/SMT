using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.PcbInstructionDto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class PcbInstructionService : IPcbInstructionService
    {
        private readonly IPcbInstructionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PcbInstructionService(IPcbInstructionRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(PcbInstructionCreate instructionCreate)
        {
            var pcbInstruction = await _repository.FindAsync(p => p.PositionId == instructionCreate.PositionId);

            if (pcbInstruction != null)
            {
                pcbInstruction.ImagePath = instructionCreate.ImagePath;
                _repository.Update(pcbInstruction);
            }
            else
            {
                pcbInstruction = _mapper.Map<PcbInstructionCreate, PcbInstruction>(instructionCreate);
                await _repository.AddAsync(pcbInstruction);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<string>> GetAsync()
        {
            var pcbInstructions = await _repository.GetAllAsync();

            return pcbInstructions.Select(x => x.ImagePath);
        }

        public async Task<string> GetAsync(int id)
        {
            return (await _repository.FindAsync(x => x.Id == id)).ImagePath;
        }

        public async Task<string> GetByPositionAsync(int positionId)
        {
            return (await _repository.FindAsync(x => x.PositionId == positionId)).ImagePath;
        }
    }
}
