using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain.BoardFlow;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.QrReaderDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.BoardFlow
{
    public class QrReaderService : IQrReaderService
    {
        private readonly IQrReaderRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QrReaderService(IQrReaderRepository repository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<QrReaderResponse> AddAsync(QrReaderCreate qrReaderCreate)
        {
            var qrReader = await _repository.FindAsync(p => (p.Name == qrReaderCreate.Name || p.Position == qrReaderCreate.Position) && p.IsActive);

            if (qrReader != null)
                throw new ConflictException($"Name: {qrReaderCreate.Name} or Position: {qrReaderCreate.Position} already exists");

            qrReader = _mapper.Map<QrReaderCreate, QrReader>(qrReaderCreate);

            await _repository.AddAsync(qrReader);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<QrReader, QrReaderResponse>(qrReader);
        }

        public async Task<QrReaderResponse> DeleteAsync(int id)
        {
            var qrReader = await _repository.FindAsync(p => p.Id == id);

            if (qrReader == null)
                throw new NotFoundException("Not found");

            qrReader.IsActive = false;
            _repository.Update(qrReader);

            await _unitOfWork.SaveAsync();

            return _mapper.Map<QrReader, QrReaderResponse>(qrReader);
        }

        public async Task<IEnumerable<QrReaderResponse>> GetAllAsync(bool? isActive)
        {
            var qrReaders = await _repository.GetByAsync(x => x.IsActive == isActive);

            return _mapper.Map<IEnumerable<QrReader>, IEnumerable<QrReaderResponse>>(qrReaders);
        }

        public async Task<QrReaderResponse> GetAsync(int id)
        {
            var brand = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<QrReader, QrReaderResponse>(brand);
        }

        public async Task<QrReaderResponse> GetByNameAsync(string name)
        {
            var brand = await _repository.FindAsync(p => p.Name == name);

            return _mapper.Map<QrReader, QrReaderResponse>(brand);
        }

        public async Task<QrReaderResponse> UpdateAsync(int id, QrReaderUpdate qrReaderUpdate)
        {
            var qrReader = await _repository.FindAsync(p => p.Id == id);

            if (qrReader == null)
                throw new NotFoundException("Not found");

            qrReader = await _repository.FindAsync(x => (x.Position == qrReaderUpdate.Position || x.Name == qrReaderUpdate.Name) && x.IsActive);

            if (qrReader != null)
                throw new ConflictException($"Name or Position already occupied");

            qrReader.Name = qrReaderUpdate.Name;
            qrReader.Position = qrReaderUpdate.Position;

            _repository.Update(qrReader);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<QrReader, QrReaderResponse>(qrReader);
        }
    }
}
