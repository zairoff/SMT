using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.ModelInstructionImageDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class ModelInstructionImageService : IModelInstructionImageService
    {
        private readonly IModelInstructionImageRepository _repository;
        private readonly IInstructionPositionRepository _positionRepository;
        private readonly ILineActiveModelRepository _lineActiveModelRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ModelInstructionImageService(
            IModelInstructionImageRepository repository,
            IInstructionPositionRepository positionRepository,
            ILineActiveModelRepository lineActiveModelRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _positionRepository = positionRepository;
            _lineActiveModelRepository = lineActiveModelRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ModelInstructionImageResponse> AddOrUpdateAsync(ModelInstructionImageCreate instructionImageCreate)
        {
            var instructionImage = await _repository.FindAsync(i =>
                i.ModelId == instructionImageCreate.ModelId &&
                i.InstructionPositionId == instructionImageCreate.InstructionPositionId);

            if (instructionImage != null)
            {
                instructionImage.ImagePath = instructionImageCreate.ImagePath;
                _repository.Update(instructionImage);
            }
            else
            {
                instructionImage = _mapper.Map<ModelInstructionImageCreate, ModelInstructionImage>(instructionImageCreate);
                await _repository.AddAsync(instructionImage);
            }

            await _unitOfWork.SaveAsync();

            instructionImage = await _repository.FindAsync(i =>
                i.ModelId == instructionImageCreate.ModelId &&
                i.InstructionPositionId == instructionImageCreate.InstructionPositionId);

            return _mapper.Map<ModelInstructionImage, ModelInstructionImageResponse>(instructionImage);
        }

        public async Task<ModelInstructionImageResponse> DeleteAsync(int id)
        {
            var instructionImage = await _repository.FindAsync(i => i.Id == id);

            if (instructionImage == null)
                throw new NotFoundException("Not found");

            _repository.Delete(instructionImage);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ModelInstructionImage, ModelInstructionImageResponse>(instructionImage);
        }

        public async Task<IEnumerable<ModelInstructionImageResponse>> GetByModelAsync(int modelId)
        {
            var instructionImages = await _repository.GetByAsync(i => i.ModelId == modelId);

            return _mapper.Map<IEnumerable<ModelInstructionImage>, IEnumerable<ModelInstructionImageResponse>>(instructionImages);
        }

        public async Task<IEnumerable<ModelInstructionImageResponse>> GetByPositionAsync(int positionId)
        {
            var instructionImages = await _repository.GetByAsync(i => i.InstructionPositionId == positionId);

            return _mapper.Map<IEnumerable<ModelInstructionImage>, IEnumerable<ModelInstructionImageResponse>>(instructionImages);
        }

        public async Task<CurrentInstructionResponse> GetCurrentByPositionAsync(int positionId)
        {
            var position = await _positionRepository.FindAsync(p => p.Id == positionId);

            if (position == null)
            {
                return new CurrentInstructionResponse
                {
                    PositionId = positionId,
                    HasActiveModel = false,
                    HasImage = false,
                    Message = "Position not found",
                };
            }

            var response = new CurrentInstructionResponse
            {
                PositionId = position.Id,
                PositionName = position.Name,
            };

            var activeModel = await _lineActiveModelRepository.GetByLineAsync(position.LineId);

            if (activeModel == null)
            {
                response.HasActiveModel = false;
                response.HasImage = false;
                response.Message = "No active model set for this line";

                return response;
            }

            response.HasActiveModel = true;
            response.ModelId = activeModel.ModelId;
            response.ModelName = activeModel.Model?.Name;

            var instructionImage = await _repository.FindAsync(i =>
                i.ModelId == activeModel.ModelId &&
                i.InstructionPositionId == positionId);

            if (instructionImage == null)
            {
                response.HasImage = false;
                response.Message = "No image configured for this position";

                return response;
            }

            response.HasImage = true;
            response.ImagePath = instructionImage.ImagePath;

            return response;
        }
    }
}
