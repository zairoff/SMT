using AutoMapper;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain.BoardFlow;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces;
using SMT.ViewModel.Dto.BoardReportDto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.BoardFlow
{
    public class BoardReportService : IBoardReportService
    {
        private readonly IBoardReportRepository _repository;
        private readonly IModelRepository _modelRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BoardReportService(IBoardReportRepository repository, IMapper mapper, IUnitOfWork unitOfWork, IModelRepository modelRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _modelRepository = modelRepository;
        }

        public async Task<BoardReportResponse> AddAsync(BoardReportCreate boardReportCreate)
        {
            if (string.IsNullOrEmpty(boardReportCreate.QrCode) || boardReportCreate.QrCode.Length != 14)
            {
                throw new InvalidOperationException("Invalid QR code");
            }

            var barcode = boardReportCreate.QrCode[..6];

            var model = await _modelRepository.FindAsync(x => x.Barcode == barcode);

            if (model == null)
            {
                throw new NotFoundException("Model not found");
            }

            BoardReport boardReport;
            if (boardReportCreate.QrReaderPositionId == 1)
            {
                boardReport = await _repository.FindAsync(x => x.QrCode == boardReportCreate.QrCode);

                if (boardReport != null)
                {
                    throw new ConflictException($"{boardReportCreate.QrCode} already exists");
                }
            }
            else
            {
                boardReport = await _repository.FindAsync(x => x.QrCode == boardReportCreate.QrCode && x.QrReader.Position == boardReportCreate.QrReaderPreviousPositionId);

                if (boardReport == null)
                {
                    // TODO: Notify

                    // That means, the board did not pass from previous reader (missing prevous reader check)
                    boardReport = new BoardReport
                    {
                        ModelId = model.Id,
                        QrCode = boardReportCreate.QrCode,
                        QrReaderId = boardReportCreate.QrReaderId,
                        Status = BoardPassStatus.MissingPreviousPass,
                        DateTime = DateTime.Now,
                    };

                    await _repository.AddAsync(boardReport);
                }
                else
                {
                    boardReport = new BoardReport
                    {
                        ModelId = model.Id,
                        QrCode = boardReportCreate.QrCode,
                        QrReaderId = boardReportCreate.QrReaderId,
                        Status = BoardPassStatus.Passed,
                        DateTime = DateTime.Now,
                    };

                    await _repository.AddAsync(boardReport);
                }
            }

            await _unitOfWork.SaveAsync();

            return _mapper.Map<BoardReportResponse>(boardReport);
        }

        public async Task<BoardReportResponse> DeleteAsync(int id)
        {
            var component = await _repository.FindAsync(p => p.Id == id);

            if (component == null)
                throw new NotFoundException("Not found");

            component.Status = BoardPassStatus.Deleted;

            _repository.Update(component);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<BoardReportResponse>(component);
        }

        public async Task<BoardReportResponse> GetAsync(int id)
        {
            var component = await _repository.FindAsync(p => p.Id == id);

            return _mapper.Map<BoardReportResponse>(component);
        }

        public async Task<IEnumerable<BoardReportResponse>> GetByBarcodeAsync(string barcode)
        {
            var component = await _repository.GetByAsync(p => p.QrCode == barcode);

            return _mapper.Map<IEnumerable<BoardReportResponse>>(component);
        }

        public async Task<IEnumerable<BoardReportResponse>> GetByReaderAsync(int readerId, DateTime from, DateTime to)
        {
            var component = await _repository.GetByAsync(p => p.QrReaderId == readerId && p.DateTime.Date >= from.Date && p.DateTime.Date <= to.Date);

            return _mapper.Map<IEnumerable<BoardReportResponse>>(component);
        }
    }
}
