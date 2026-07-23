using AutoMapper;
using SMT.Access.Repository.BoardFlowV2;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain.BoardFlow.V2;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces.BoardFlowV2;
using SMT.ViewModel.Dto.BoardMovementV2Dto;
using SMT.ViewModel.Dto.BoardV2Dto;
using SMT.ViewModel.Models.BoardFlowV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services.BoardFlowV2
{
    public class BoardV2Service : IBoardV2Service
    {
        private readonly IBoardV2Repository _boardRepository;
        private readonly IBoardMovementV2Repository _movementRepository;
        private readonly IQrReaderV2Repository _readerRepository;
        private readonly IQrReaderV2LinkRepository _linkRepository;
        private readonly IModelRepository _modelRepository;
        private readonly ILineRepository _lineRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BoardV2Service(
            IBoardV2Repository boardRepository,
            IBoardMovementV2Repository movementRepository,
            IQrReaderV2Repository readerRepository,
            IQrReaderV2LinkRepository linkRepository,
            IModelRepository modelRepository,
            ILineRepository lineRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _boardRepository = boardRepository;
            _movementRepository = movementRepository;
            _readerRepository = readerRepository;
            _linkRepository = linkRepository;
            _modelRepository = modelRepository;
            _lineRepository = lineRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // The core state machine: every scan either creates a board at an entry
        // station, advances it to the next station, or - if the board already has
        // a known position and this scan doesn't follow from it (skipped a station,
        // went backwards, or - the case that matters most here - a board that
        // already went down one branch, e.g. PCBA-1, getting scanned into a
        // mutually exclusive branch like PCBA-2) - the scan is rejected outright
        // rather than silently moving the board. A never-before-seen board scanned
        // at a non-entry station is still accepted-and-flagged, since rejecting it
        // would mean it never gets tracked at all.
        public async Task<BoardV2Response> ScanAsync(BoardV2Create create)
        {
            if (string.IsNullOrEmpty(create.QrCode) || create.QrCode.Length != 14)
                throw new InvalidOperationException("Invalid QR code");

            var reader = await _readerRepository.FindAsync(r => r.Id == create.QrReaderId);

            if (reader == null || !reader.IsActive)
                throw new NotFoundException("Reader not found");

            var barcode = create.QrCode[..5];
            var model = await _modelRepository.FindAsync(m => m.Barcode == barcode);

            if (model == null)
                throw new NotFoundException("Model not found");

            var board = await _boardRepository.FindByQrCodeAsync(create.QrCode);
            var upstreamReaderIds = (await _linkRepository.GetByToReaderAsync(reader.Id))
                .Select(l => l.FromReaderId)
                .ToHashSet();
            var isEntryStation = upstreamReaderIds.Count == 0;

            var now = DateTime.Now;
            var movementStatus = BoardMovementStatusV2.Passed;
            var evaluateCompletion = false;

            if (board == null)
            {
                board = new BoardV2
                {
                    QrCode = create.QrCode,
                    ModelId = model.Id,
                    LineId = reader.LineId,
                    CurrentQrReaderId = reader.Id,
                    Status = isEntryStation ? BoardStatusV2.InProgress : BoardStatusV2.Flagged,
                    StartedAt = now,
                    UpdatedAt = now,
                };

                movementStatus = isEntryStation ? BoardMovementStatusV2.Passed : BoardMovementStatusV2.MissingPreviousPass;
                evaluateCompletion = isEntryStation;

                await _boardRepository.AddAsync(board);
            }
            else if (board.CurrentQrReaderId == reader.Id)
            {
                // Duplicate scan at the same station - log it, state doesn't change.
                movementStatus = BoardMovementStatusV2.Passed;
            }
            else if (board.CurrentQrReaderId.HasValue && upstreamReaderIds.Contains(board.CurrentQrReaderId.Value))
            {
                // Advancing from any one of this station's valid predecessors counts as
                // normal progress - this is what lets a convergence station (e.g. the PCB
                // line's entry, fed by both the Parmi and Jutze SMD lines) accept boards
                // from either upstream line without being flagged.
                board.CurrentQrReaderId = reader.Id;
                board.LineId = reader.LineId;
                board.Status = BoardStatusV2.InProgress;
                board.UpdatedAt = now;

                movementStatus = BoardMovementStatusV2.Passed;
                evaluateCompletion = true;

                _boardRepository.Update(board);
            }
            else
            {
                // The board already has a known position that isn't a valid
                // predecessor of this station - reject the scan instead of moving
                // it. Still record the attempt in the movement log for audit
                // purposes, since someone will want to know a bad scan happened.
                var rejectedMovement = new BoardMovementV2
                {
                    Board = board,
                    QrReaderId = reader.Id,
                    DateTime = now,
                    Status = BoardMovementStatusV2.MissingPreviousPass,
                };

                await _movementRepository.AddAsync(rejectedMovement);
                await _unitOfWork.SaveAsync();

                var currentReaderName = board.CurrentQrReader != null
                    ? $"{board.CurrentQrReader.Line?.Name} / {board.CurrentQrReader.Name}"
                    : "an unknown station";

                throw new ConflictException(
                    $"Board {board.QrCode} is currently at {currentReaderName} and cannot be scanned at {reader.Line?.Name} / {reader.Name}.");
            }

            if (evaluateCompletion)
            {
                var hasNextStation = await _linkRepository.HasOutgoingAsync(reader.Id);

                if (!hasNextStation)
                {
                    board.Status = BoardStatusV2.Completed;
                    board.CompletedAt = now;
                }
            }

            var movement = new BoardMovementV2
            {
                Board = board,
                QrReaderId = reader.Id,
                DateTime = now,
                Status = movementStatus,
            };

            await _movementRepository.AddAsync(movement);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<BoardV2Response>(board);
        }

        public async Task<LineFlowSnapshot> GetLineSnapshotAsync(int lineId, DateTime from, DateTime to)
        {
            var line = await _lineRepository.FindAsync(l => l.Id == lineId);
            var readers = (await _readerRepository.GetByAsync(r => r.LineId == lineId && r.IsActive))
                .OrderBy(r => r.Position)
                .ToList();

            var counts = await _boardRepository.GetStationCountsAsync(lineId);
            var completedCount = await _boardRepository.GetCompletedCountAsync(lineId, from, to);

            var stations = readers.Select(r => new StationSnapshot
            {
                ReaderId = r.Id,
                ReaderName = r.Name,
                Position = r.Position,
                InProgressCount = counts.Where(c => c.ReaderId == r.Id && c.Status == BoardStatusV2.InProgress).Sum(c => c.Count),
                FlaggedCount = counts.Where(c => c.ReaderId == r.Id && c.Status == BoardStatusV2.Flagged).Sum(c => c.Count),
            }).ToList();

            return new LineFlowSnapshot
            {
                LineId = lineId,
                LineName = line?.Name,
                Stations = stations,
                CompletedCount = completedCount,
            };
        }

        // The whole-process view: every configured line's stations in one call,
        // instead of one round-trip per line.
        public async Task<IEnumerable<LineFlowSnapshot>> GetAllLineSnapshotsAsync(DateTime from, DateTime to)
        {
            var readers = (await _readerRepository.GetByAsync(r => r.IsActive)).ToList();
            var counts = await _boardRepository.GetAllStationCountsAsync();
            var completedByLine = await _boardRepository.GetAllCompletedCountsAsync(from, to);

            return readers
                .GroupBy(r => r.LineId)
                .Select(g => new LineFlowSnapshot
                {
                    LineId = g.Key,
                    LineName = g.First().Line?.Name,
                    Stations = g.OrderBy(r => r.Position).Select(r => new StationSnapshot
                    {
                        ReaderId = r.Id,
                        ReaderName = r.Name,
                        Position = r.Position,
                        InProgressCount = counts.Where(c => c.ReaderId == r.Id && c.Status == BoardStatusV2.InProgress).Sum(c => c.Count),
                        FlaggedCount = counts.Where(c => c.ReaderId == r.Id && c.Status == BoardStatusV2.Flagged).Sum(c => c.Count),
                    }).ToList(),
                    CompletedCount = completedByLine.TryGetValue(g.Key, out var completed) ? completed : 0,
                })
                .OrderBy(s => s.LineId)
                .ToList();
        }

        public async Task<IEnumerable<BoardV2Response>> GetFlaggedAsync(int? lineId)
        {
            var boards = await _boardRepository.GetByAsync(b =>
                b.Status == BoardStatusV2.Flagged && (lineId == null || b.LineId == lineId));

            return _mapper.Map<IEnumerable<BoardV2Response>>(boards);
        }

        // The actual boards behind a station's live counts (InProgress + Flagged),
        // for drilling in from the whole-process view.
        public async Task<IEnumerable<BoardV2Response>> GetBoardsAtStationAsync(int readerId)
        {
            var boards = await _boardRepository.GetByAsync(b =>
                b.CurrentQrReaderId == readerId
                && (b.Status == BoardStatusV2.InProgress || b.Status == BoardStatusV2.Flagged));

            return _mapper.Map<IEnumerable<BoardV2Response>>(boards);
        }

        public async Task<BoardHistoryResponse> GetHistoryAsync(string qrCode)
        {
            var board = await _boardRepository.FindByQrCodeAsync(qrCode);

            if (board == null)
                throw new NotFoundException("Board not found");

            var movements = await _movementRepository.GetByBoardAsync(board.Id);

            return new BoardHistoryResponse
            {
                Board = _mapper.Map<BoardV2Response>(board),
                Movements = _mapper.Map<List<BoardMovementV2Response>>(movements),
            };
        }

        // Backs the scan-report screen's log so it survives a page refresh -
        // it's read straight from the movement log, not kept client-side only.
        public async Task<IEnumerable<RecentMovementResponse>> GetRecentMovementsAsync(DateTime date)
        {
            var movements = await _movementRepository.GetByDateAsync(date);

            return movements.Select(m => new RecentMovementResponse
            {
                Id = m.Id,
                QrCode = m.Board?.QrCode,
                ModelName = m.Board?.Model?.Name,
                QrReaderId = m.QrReaderId,
                QrReaderName = m.QrReader?.Name,
                LineName = m.QrReader?.Line?.Name,
                DateTime = m.DateTime,
                Status = m.Status,
            }).ToList();
        }
    }
}
