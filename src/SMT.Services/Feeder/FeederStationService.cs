using SMT.Access.Repository.Feeder;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Domain.Feeder;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces.Feeder;
using SMT.ViewModel.Dto.FeederStationDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services.Feeder
{
    // Operator station: the operator scans a reel, sees which module/feeder it goes to, then scans the
    // sticker of the slot they put it in. The reel is matched by RCode against the line's active setup,
    // so any alternate part number of the same component is accepted (flagged as an alternate).
    public class FeederStationService : IFeederStationService
    {
        private const int MaxRawCodeLength = 450;
        private const int MaxRecentScans = 200;

        private readonly IFeederSetupRepository _setupRepository;
        private readonly IFeederSlotRepository _slotRepository;
        private readonly IFeederScanLogRepository _scanLogRepository;
        private readonly ILineMachineRepository _machineRepository;
        private readonly ILineActiveModelRepository _lineActiveModelRepository;
        private readonly ILineRepository _lineRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FeederStationService(IFeederSetupRepository setupRepository, IFeederSlotRepository slotRepository,
            IFeederScanLogRepository scanLogRepository, ILineMachineRepository machineRepository,
            ILineActiveModelRepository lineActiveModelRepository, ILineRepository lineRepository,
            IComponentRepository componentRepository, IUnitOfWork unitOfWork)
        {
            _setupRepository = setupRepository;
            _slotRepository = slotRepository;
            _scanLogRepository = scanLogRepository;
            _machineRepository = machineRepository;
            _lineActiveModelRepository = lineActiveModelRepository;
            _lineRepository = lineRepository;
            _componentRepository = componentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<FeederStationResponse> GetStationAsync(int lineId)
        {
            var line = await _lineRepository.FindAsync(l => l.Id == lineId);
            if (line == null)
                throw new NotFoundException($"Line {lineId} not found");

            var machine = await _machineRepository.GetByLineAsync(lineId);
            var activeModel = await _lineActiveModelRepository.GetByLineAsync(lineId);
            var setups = await _setupRepository.GetByAsync(s => s.LineId == lineId && s.Status == FeederSetupStatus.Active);

            return new FeederStationResponse
            {
                LineId = line.Id,
                LineName = line.Name,
                MachineName = machine?.Name,
                ActiveModelId = activeModel?.ModelId,
                Setups = setups
                    .OrderBy(s => s.Model.Name)
                    .ThenBy(s => s.Side)
                    .Select(s => new FeederStationSetup
                    {
                        SetupId = s.Id,
                        ModelId = s.ModelId,
                        ModelName = s.Model.Name,
                        Side = s.Side,
                        Version = s.Version,
                        ItemCount = s.Items.Count,
                    })
                    .ToList(),
            };
        }

        public async Task<FeederScanResponse> LookupAsync(FeederReelScan scan, string username)
        {
            var setup = await GetActiveSetupAsync(scan);
            var reel = await ResolveReelAsync(setup, scan.ReelCode);

            if (reel.Response.Result == FeederScanResult.Ng)
            {
                await LogAsync(scan, setup, null, reel, null, username);
                await _unitOfWork.SaveAsync();
            }

            return reel.Response;
        }

        public async Task<FeederScanResponse> VerifyAsync(FeederReelScan scan, string username)
        {
            var setup = await GetActiveSetupAsync(scan);
            var reel = await ResolveReelAsync(setup, scan.ReelCode);
            var response = reel.Response;

            FeederSlot slot = null;
            int? expectedComponentId = reel.Component?.Id;

            if (response.Result != FeederScanResult.Ng)
            {
                slot = FeederCodes.IsSlotCode(scan.SlotCode)
                    ? await _slotRepository.GetBySlotCodeAsync(FeederCodes.NormalizeCode(scan.SlotCode))
                    : null;

                if (slot != null)
                    response.ScannedSlot = ToTarget(slot);

                if (slot == null)
                    Fail(response, FeederNgReason.UnknownSlot);
                else if (slot.MachineModule.LineMachine.LineId != scan.LineId)
                    Fail(response, FeederNgReason.WrongLine);
                else if (response.Targets.All(t => t.FeederSlotId != slot.Id))
                {
                    var slotItem = setup.Items.FirstOrDefault(i => i.FeederSlotId == slot.Id);
                    response.ScannedSlotExpectedRCode = slotItem?.Component.RCode;
                    expectedComponentId = slotItem?.ComponentId;
                    Fail(response, FeederNgReason.WrongSlot);
                }
                else
                    await LoadReelIntoSlotAsync(slot, reel, scan.ReelCode, username);
            }

            await LogAsync(scan, setup, slot, reel, expectedComponentId, username);
            await _unitOfWork.SaveAsync();

            return response;
        }

        public async Task<IEnumerable<FeederScanLogResponse>> GetRecentScansAsync(int lineId, int take)
        {
            var logs = await _scanLogRepository.GetRecentAsync(lineId, Math.Clamp(take, 1, MaxRecentScans));

            return logs.Select(l => new FeederScanLogResponse
            {
                Id = l.Id,
                ScanType = l.ScanType,
                Result = l.Result,
                NgReason = l.NgReason,
                RawReelCode = l.RawReelCode,
                RawSlotCode = l.RawSlotCode,
                ModuleName = l.FeederSlot?.MachineModule?.Name,
                SlotNumber = l.FeederSlot?.Number,
                ExpectedRCode = l.ExpectedComponent?.RCode,
                ScannedRCode = l.ScannedComponent?.RCode,
                ScannedSpecification = l.ScannedComponent?.Specification,
                ReelPartNumber = l.ReelPartNumber,
                Username = l.Username,
                CreatedAt = l.CreatedAt,
            }).ToList();
        }

        // The station holds on to a setup id; if that setup was replaced (a newer version activated)
        // or doesn't belong to the line, scans must not be verified against it.
        private async Task<FeederSetup> GetActiveSetupAsync(FeederReelScan scan)
        {
            var setup = await _setupRepository.GetWithItemsAsync(scan.SetupId);

            return setup != null && setup.Status == FeederSetupStatus.Active && setup.LineId == scan.LineId
                ? setup
                : null;
        }

        private async Task<ResolvedReel> ResolveReelAsync(FeederSetup setup, string rawReelCode)
        {
            var parsed = ReelBarcodeParser.Parse(rawReelCode);
            var reel = new ResolvedReel
            {
                Parsed = parsed,
                Response = new FeederScanResponse
                {
                    Result = FeederScanResult.Ok,
                    ReelPartNumber = parsed.PartNumber,
                    ScannedAt = DateTime.Now,
                },
            };

            if (setup == null)
                return Fail(reel, FeederNgReason.NoActiveSetup);

            if (parsed.Candidates.Count == 0)
                return Fail(reel, FeederNgReason.UnknownReel);

            var setupIndex = new ComponentIndex(setup.Items.Select(i => i.Component).GroupBy(c => c.Id).Select(g => g.First()));
            var lookup = setupIndex.Resolve(parsed.Candidates);

            if (lookup.Component == null)
            {
                // Only on the NG path: tell "not in this setup" apart from "not in the catalog at all".
                var catalogLookup = new ComponentIndex(await _componentRepository.GetActiveAsync()).Resolve(parsed.Candidates);
                SetComponent(reel, catalogLookup.Component);

                return Fail(reel, catalogLookup.Component != null ? FeederNgReason.NotInSetup : FeederNgReason.UnknownReel);
            }

            SetComponent(reel, lookup.Component);

            var items = setup.Items.Where(i => i.ComponentId == lookup.Component.Id).ToList();
            reel.Response.Targets = items
                .OrderBy(i => i.FeederSlot.MachineModule.Order)
                .ThenBy(i => i.FeederSlot.Number, FeederSlotNumberComparer.Instance)
                .Select(i => ToTarget(i.FeederSlot))
                .ToList();

            var reelPartNumber = parsed.PartNumber ?? (lookup.Match == ComponentMatch.RCode ? null : lookup.MatchedCode);
            reel.Response.IsAlternate = items.Any(i => IsAlternate(reelPartNumber, i));
            if (reel.Response.IsAlternate)
                reel.Response.Result = FeederScanResult.OkAlternate;

            return reel;
        }

        // Alternate = the reel's part number differs from the part number the setup was imported with.
        // A setup imported by RCode doesn't prefer any part number, so nothing is an alternate there.
        private static bool IsAlternate(string reelPartNumber, FeederSetupItem item)
        {
            if (reelPartNumber == null)
                return false;

            var imported = ReelBarcodeParser.Parse(item.ImportedCode);
            var setupPartNumber = imported.PartNumber
                ?? imported.Candidates.FirstOrDefault(c => c != FeederCodes.NormalizeCode(item.Component.RCode));

            return setupPartNumber != null && setupPartNumber != FeederCodes.NormalizeCode(reelPartNumber);
        }

        // A reel can only be in one place: loading it into this slot takes it out of wherever it was
        // recorded before (e.g. moved from another feeder during a changeover).
        private async Task LoadReelIntoSlotAsync(FeederSlot slot, ResolvedReel reel, string rawReelCode, string username)
        {
            var reelCode = Truncate(rawReelCode?.Trim());

            foreach (var previous in await _slotRepository.GetByCurrentReelCodeAsync(reelCode))
            {
                if (previous.Id == slot.Id)
                    continue;

                previous.CurrentComponentId = null;
                previous.CurrentReelCode = null;
                previous.LoadedBy = null;
                previous.LoadedAt = null;
            }

            slot.CurrentComponentId = reel.Component.Id;
            slot.CurrentReelCode = reelCode;
            slot.LoadedBy = username;
            slot.LoadedAt = DateTime.Now;
        }

        private async Task LogAsync(FeederReelScan scan, FeederSetup setup, FeederSlot slot, ResolvedReel reel,
            int? expectedComponentId, string username)
        {
            await _scanLogRepository.AddAsync(new FeederScanLog
            {
                LineId = scan.LineId,
                ScanType = FeederScanType.Replacement,
                FeederSetupId = setup?.Id,
                FeederSlotId = slot?.Id,
                RawReelCode = Truncate(scan.ReelCode?.Trim()),
                RawSlotCode = Truncate(scan.SlotCode?.Trim()),
                ReelPartNumber = Truncate(reel.Parsed.PartNumber, 256),
                ExpectedComponentId = expectedComponentId,
                ScannedComponentId = reel.Component?.Id,
                Result = reel.Response.Result,
                NgReason = reel.Response.NgReason,
                Username = username,
                CreatedAt = reel.Response.ScannedAt,
            });
        }

        private static void SetComponent(ResolvedReel reel, Component component)
        {
            reel.Component = component;
            reel.Response.ComponentId = component?.Id;
            reel.Response.RCode = component?.RCode;
            reel.Response.Specification = component?.Specification;
        }

        private static ResolvedReel Fail(ResolvedReel reel, FeederNgReason reason)
        {
            Fail(reel.Response, reason);
            return reel;
        }

        private static void Fail(FeederScanResponse response, FeederNgReason reason)
        {
            response.Result = FeederScanResult.Ng;
            response.NgReason = reason;
        }

        private static FeederSlotTarget ToTarget(FeederSlot slot)
        {
            return new FeederSlotTarget
            {
                FeederSlotId = slot.Id,
                SlotCode = slot.SlotCode,
                ModuleName = slot.MachineModule?.Name,
                SlotNumber = slot.Number,
            };
        }

        private static string Truncate(string value, int maxLength = MaxRawCodeLength)
        {
            return value == null || value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        private class ResolvedReel
        {
            public ParsedReelCode Parsed { get; set; }

            public Component Component { get; set; }

            public FeederScanResponse Response { get; set; }
        }
    }
}
