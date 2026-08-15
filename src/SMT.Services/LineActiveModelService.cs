using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.LineActiveModelDto;
using System;
using System.Threading.Tasks;

namespace SMT.Services
{
    public class LineActiveModelService : ILineActiveModelService
    {
        private readonly ILineActiveModelRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LineActiveModelService(ILineActiveModelRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LineActiveModelResponse> GetByLineAsync(int lineId)
        {
            var activeModel = await _repository.GetByLineAsync(lineId);

            return _mapper.Map<LineActiveModel, LineActiveModelResponse>(activeModel);
        }

        public async Task<LineActiveModelResponse> SetActiveModelAsync(LineActiveModelSet activeModelSet)
        {
            var activeModel = await _repository.GetByLineAsync(activeModelSet.LineId);

            if (activeModel != null)
            {
                activeModel.ModelId = activeModelSet.ModelId;
                activeModel.UpdatedAt = DateTime.Now;
                _repository.Update(activeModel);
            }
            else
            {
                activeModel = new LineActiveModel
                {
                    LineId = activeModelSet.LineId,
                    ModelId = activeModelSet.ModelId,
                    UpdatedAt = DateTime.Now,
                };
                await _repository.AddAsync(activeModel);
            }

            await _unitOfWork.SaveAsync();

            return await GetByLineAsync(activeModelSet.LineId);
        }
    }
}
