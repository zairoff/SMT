using AutoMapper;
using SMT.Access.Repository.BoardFlowV2;
using SMT.Access.Unit;
using SMT.Domain.BoardFlow.V2;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces.BoardFlowV2;
using SMT.ViewModel.Dto.QrReaderV2Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services.BoardFlowV2
{
    public class QrReaderV2Service : IQrReaderV2Service
    {
        private readonly IQrReaderV2Repository _repository;
        private readonly IQrReaderV2LinkRepository _linkRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QrReaderV2Service(IQrReaderV2Repository repository, IQrReaderV2LinkRepository linkRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _linkRepository = linkRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<QrReaderV2Response> AddAsync(QrReaderV2Create create)
        {
            var existing = await _repository.FindAsync(r => r.LineId == create.LineId
                && (r.Name == create.Name || r.Position == create.Position) && r.IsActive);

            if (existing != null)
                throw new ConflictException($"Name: {create.Name} or Position: {create.Position} already exists on this line");

            var qrReader = new QrReaderV2
            {
                LineId = create.LineId,
                Name = create.Name,
                Position = create.Position,
                IsActive = true,
            };

            await _repository.AddAsync(qrReader);

            foreach (var previousReader in await ResolvePreviousReadersAsync(create.PreviousReaderIds, null))
            {
                await _linkRepository.AddAsync(new QrReaderV2Link { FromReader = previousReader, ToReader = qrReader });
            }

            await _unitOfWork.SaveAsync();

            return await BuildResponseAsync(qrReader);
        }

        public async Task<QrReaderV2Response> UpdateAsync(int id, QrReaderV2Update update)
        {
            var qrReader = await _repository.FindAsync(r => r.Id == id);

            if (qrReader == null)
                throw new NotFoundException("Not found");

            var conflict = await _repository.FindAsync(r => r.Id != id && r.LineId == update.LineId
                && (r.Name == update.Name || r.Position == update.Position) && r.IsActive);

            if (conflict != null)
                throw new ConflictException("Name or Position already occupied on this line");

            var newPreviousReaders = await ResolvePreviousReadersAsync(update.PreviousReaderIds, id);

            var existingLinks = await _linkRepository.GetByToReaderAsync(id);
            foreach (var link in existingLinks)
                _linkRepository.Delete(link);

            foreach (var previousReader in newPreviousReaders)
                await _linkRepository.AddAsync(new QrReaderV2Link { FromReader = previousReader, ToReader = qrReader });

            qrReader.LineId = update.LineId;
            qrReader.Name = update.Name;
            qrReader.Position = update.Position;

            _repository.Update(qrReader);
            await _unitOfWork.SaveAsync();

            return await BuildResponseAsync(qrReader);
        }

        public async Task<QrReaderV2Response> DeleteAsync(int id)
        {
            var qrReader = await _repository.FindAsync(r => r.Id == id);

            if (qrReader == null)
                throw new NotFoundException("Not found");

            qrReader.IsActive = false;
            _repository.Update(qrReader);

            await _unitOfWork.SaveAsync();

            return await BuildResponseAsync(qrReader);
        }

        public async Task<QrReaderV2Response> GetAsync(int id)
        {
            var qrReader = await _repository.FindAsync(r => r.Id == id);

            return qrReader == null ? null : await BuildResponseAsync(qrReader);
        }

        public async Task<IEnumerable<QrReaderV2Response>> GetAllAsync(int? lineId, bool? isActive)
        {
            var qrReaders = await _repository.GetByAsync(r =>
                (lineId == null || r.LineId == lineId) && (isActive == null || r.IsActive == isActive));

            var responses = new List<QrReaderV2Response>();
            foreach (var qrReader in qrReaders)
                responses.Add(await BuildResponseAsync(qrReader));

            return responses;
        }

        private async Task<List<QrReaderV2>> ResolvePreviousReadersAsync(List<int> previousReaderIds, int? selfId)
        {
            var result = new List<QrReaderV2>();

            foreach (var previousReaderId in (previousReaderIds ?? new List<int>()).Distinct())
            {
                if (previousReaderId == selfId)
                    throw new ConflictException("A station cannot be its own previous station");

                var previousReader = await _repository.FindAsync(r => r.Id == previousReaderId);

                if (previousReader == null || !previousReader.IsActive)
                    throw new NotFoundException($"Previous reader {previousReaderId} not found");

                result.Add(previousReader);
            }

            return result;
        }

        private async Task<QrReaderV2Response> BuildResponseAsync(QrReaderV2 qrReader)
        {
            var response = _mapper.Map<QrReaderV2Response>(qrReader);
            var incomingLinks = await _linkRepository.GetByToReaderAsync(qrReader.Id);

            response.PreviousReaders = incomingLinks.Select(l => new UpstreamReaderSummary
            {
                Id = l.FromReader.Id,
                Name = l.FromReader.Name,
                LineId = l.FromReader.LineId,
                LineName = l.FromReader.Line?.Name,
            }).ToList();

            return response;
        }
    }
}
