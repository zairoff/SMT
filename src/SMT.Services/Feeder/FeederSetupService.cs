using AutoMapper;
using SMT.Access.Repository.Feeder;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain;
using SMT.Domain.Feeder;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces.Feeder;
using SMT.ViewModel.Dto.FeederSetupDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services.Feeder
{
    public class FeederSetupService : IFeederSetupService
    {
        private readonly IFeederSetupRepository _repository;
        private readonly ILineMachineRepository _machineRepository;
        private readonly IFeederSlotRepository _slotRepository;
        private readonly IComponentRepository _componentRepository;
        private readonly ILineRepository _lineRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeederSetupService(IFeederSetupRepository repository, ILineMachineRepository machineRepository,
            IFeederSlotRepository slotRepository, IComponentRepository componentRepository, ILineRepository lineRepository,
            IModelRepository modelRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _machineRepository = machineRepository;
            _slotRepository = slotRepository;
            _componentRepository = componentRepository;
            _lineRepository = lineRepository;
            _modelRepository = modelRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FeederSetupResponse>> GetAllAsync(int? lineId, int? modelId, FeederSetupStatus? status)
        {
            var setups = await _repository.GetByAsync(s =>
                (lineId == null || s.LineId == lineId)
                && (modelId == null || s.ModelId == modelId)
                && (status == null || s.Status == status));

            return _mapper.Map<IEnumerable<FeederSetupResponse>>(setups);
        }

        public async Task<FeederSetupResponse> GetAsync(int id)
        {
            var setup = await _repository.GetWithItemsAsync(id);

            return setup == null ? null : ToDetailResponse(setup);
        }

        public async Task<FeederSetupPreview> PreviewAsync(FeederSetupImport import)
        {
            var context = await BuildImportContextAsync(import);

            return context.Preview;
        }

        public async Task<FeederSetupResponse> ImportAsync(FeederSetupImport import, string username)
        {
            var context = await BuildImportContextAsync(import);

            if (!context.Preview.CanImport)
                throw new InvalidOperationException(
                    $"Setup has {context.Preview.ErrorCount} error(s) - fix the file and preview again");

            var machine = context.Machine;
            var factory = new FeederSlotFactory(_slotRepository);

            var setup = new FeederSetup
            {
                ModelId = import.ModelId,
                LineId = import.LineId,
                Side = import.Side,
                Version = context.Preview.NextVersion,
                Status = FeederSetupStatus.Draft,
                SourceFileName = import.SourceFileName,
                CreatedBy = username,
                CreatedAt = DateTime.Now,
            };

            foreach (var row in context.Rows)
            {
                var module = machine.Modules.FirstOrDefault(m => m.Name == row.Module);
                if (module == null)
                {
                    module = new MachineModule
                    {
                        Name = row.Module,
                        Order = machine.Modules.Count == 0 ? 1 : machine.Modules.Max(m => m.Order) + 1,
                    };
                    machine.Modules.Add(module);
                }

                var slot = module.Slots.FirstOrDefault(s => s.Number == row.Number)
                    ?? await factory.CreateAsync(machine.LineId, module, row.Number);

                // Components come from an untracked query - set the key only, never the navigation,
                // or EF would try to insert them.
                setup.Items.Add(new FeederSetupItem
                {
                    FeederSlot = slot,
                    ComponentId = row.Component.Id,
                    ImportedCode = row.Code,
                });
            }

            await _repository.AddAsync(setup);
            await _unitOfWork.SaveAsync();

            return await GetAsync(setup.Id);
        }

        // Activating archives the currently active version of the same model/line/side. An archived
        // version can be activated again to roll back.
        public async Task<FeederSetupResponse> ActivateAsync(int id, string username)
        {
            var setup = await _repository.FindAsync(s => s.Id == id);
            if (setup == null)
                throw new NotFoundException($"Feeder setup {id} not found");

            if (setup.Status == FeederSetupStatus.Active)
                throw new ConflictException("Setup is already active");

            var currentlyActive = await _repository.GetByAsync(s => s.ModelId == setup.ModelId
                && s.LineId == setup.LineId && s.Side == setup.Side && s.Status == FeederSetupStatus.Active);

            foreach (var active in currentlyActive)
                active.Status = FeederSetupStatus.Archived;

            setup.Status = FeederSetupStatus.Active;
            setup.ActivatedBy = username;
            setup.ActivatedAt = DateTime.Now;

            await _unitOfWork.SaveAsync();

            return await GetAsync(id);
        }

        // Without againstId: compares with the active version, or - when this one is the active
        // version - with the latest version before it.
        public async Task<FeederSetupDiff> DiffAsync(int id, int? againstId)
        {
            var setup = await _repository.GetWithItemsAsync(id);
            if (setup == null)
                throw new NotFoundException($"Feeder setup {id} not found");

            FeederSetup against;
            if (againstId != null)
            {
                against = await _repository.GetWithItemsAsync(againstId.Value);
                if (against == null)
                    throw new NotFoundException($"Feeder setup {againstId} not found");
            }
            else
            {
                against = await _repository.GetActiveAsync(setup.ModelId, setup.LineId, setup.Side);

                if (against == null || against.Id == setup.Id)
                {
                    var previous = (await _repository.GetByAsync(s => s.ModelId == setup.ModelId
                            && s.LineId == setup.LineId && s.Side == setup.Side && s.Version < setup.Version))
                        .OrderByDescending(s => s.Version)
                        .FirstOrDefault();

                    against = previous == null ? null : await _repository.GetWithItemsAsync(previous.Id);
                }
            }

            return BuildDiff(setup, against);
        }

        // Re-imports the source setup's items (by RCode) onto another line's machine, matching
        // modules/slots by name and number - so it goes through the same validation as an Excel import.
        public async Task<FeederSetupResponse> CopyToLineAsync(int id, FeederSetupCopy copy, string username)
        {
            var source = await _repository.GetWithItemsAsync(id);
            if (source == null)
                throw new NotFoundException($"Feeder setup {id} not found");

            if (source.LineId == copy.TargetLineId)
                throw new InvalidOperationException("Setup already belongs to this line");

            var import = new FeederSetupImport
            {
                LineId = copy.TargetLineId,
                ModelId = source.ModelId,
                Side = source.Side,
                SourceFileName = $"Copy of {source.Line?.Name} v{source.Version}",
                Rows = source.Items
                    .Select((item, index) => new FeederSetupImportRow
                    {
                        RowNumber = index + 1,
                        Module = item.FeederSlot.MachineModule.Name,
                        Feeder = item.FeederSlot.Number,
                        Code = !string.IsNullOrWhiteSpace(item.Component.RCode)
                            ? item.Component.RCode
                            : item.Component.PartNumber?.FirstOrDefault(),
                    })
                    .ToList(),
            };

            return await ImportAsync(import, username);
        }

        public async Task<FeederSetupResponse> DeleteAsync(int id)
        {
            var setup = await _repository.GetWithItemsAsync(id);
            if (setup == null)
                throw new NotFoundException($"Feeder setup {id} not found");

            if (setup.Status != FeederSetupStatus.Draft)
                throw new ConflictException("Only draft setups can be deleted");

            var response = ToDetailResponse(setup);

            _repository.Delete(setup);
            await _unitOfWork.SaveAsync();

            return response;
        }

        private async Task<ImportContext> BuildImportContextAsync(FeederSetupImport import)
        {
            if (!Enum.IsDefined(typeof(BoardSide), import.Side))
                throw new InvalidOperationException($"Unknown board side: {import.Side}");

            if (await _lineRepository.FindAsync(l => l.Id == import.LineId) == null)
                throw new NotFoundException($"Line {import.LineId} not found");

            if (await _modelRepository.FindAsync(m => m.Id == import.ModelId) == null)
                throw new NotFoundException($"Model {import.ModelId} not found");

            var machine = await _machineRepository.GetByLineAsync(import.LineId);
            var index = new ComponentIndex(await _componentRepository.GetActiveAsync());
            var active = await _repository.GetActiveAsync(import.ModelId, import.LineId, import.Side);

            var preview = new FeederSetupPreview
            {
                LineId = import.LineId,
                ModelId = import.ModelId,
                Side = import.Side,
                NextVersion = await _repository.GetLatestVersionAsync(import.ModelId, import.LineId, import.Side) + 1,
                ActiveVersion = active?.Version,
            };

            if (machine == null)
                preview.GlobalErrors.Add(FeederSetupIssues.NoMachine);

            var rows = new List<ResolvedRow>();

            foreach (var source in (import.Rows ?? new List<FeederSetupImportRow>()).Where(r => !IsBlank(r)))
            {
                var row = new ResolvedRow
                {
                    Module = FeederCodes.NormalizeModule(source.Module),
                    Number = FeederCodes.NormalizeSlotNumber(source.Feeder),
                    Code = source.Code?.Trim(),
                    Preview = new FeederSetupPreviewRow
                    {
                        RowNumber = source.RowNumber,
                        Module = source.Module,
                        Feeder = source.Feeder,
                        Code = source.Code,
                    },
                };
                rows.Add(row);

                if (row.Module.Length == 0 || row.Number.Length == 0 || string.IsNullOrEmpty(row.Code))
                {
                    row.Preview.Issues.Add(FeederSetupIssues.MissingValue);
                    continue;
                }

                var lookup = index.Resolve(ReelBarcodeParser.Parse(row.Code).Candidates);
                if (lookup.Match == ComponentMatch.None)
                    row.Preview.Issues.Add(FeederSetupIssues.UnknownCode);
                else if (lookup.Match == ComponentMatch.Ambiguous)
                    row.Preview.Issues.Add(FeederSetupIssues.AmbiguousCode);
                else
                {
                    row.Component = lookup.Component;
                    row.Preview.ComponentId = lookup.Component.Id;
                    row.Preview.RCode = lookup.Component.RCode;
                    row.Preview.Specification = lookup.Component.Specification;
                }

                var module = machine?.Modules.FirstOrDefault(m => m.Name == row.Module);
                var slot = module?.Slots.FirstOrDefault(s => s.Number == row.Number);

                if (module == null)
                    row.Preview.Issues.Add(FeederSetupIssues.NewModule);
                else if (slot == null)
                    row.Preview.Issues.Add(FeederSetupIssues.NewSlot);
                else if (!slot.IsActive)
                    row.Preview.Issues.Add(FeederSetupIssues.InactiveSlot);

                // For new slots this is the expected code; the factory may add a suffix on collision.
                row.Preview.SlotCode = slot?.SlotCode ?? FeederCodes.BuildSlotCode(import.LineId, row.Module, row.Number);
            }

            if (rows.Count == 0)
                preview.GlobalErrors.Add(FeederSetupIssues.NoRows);

            foreach (var group in rows.Where(r => r.Module.Length > 0 && r.Number.Length > 0)
                .GroupBy(r => (r.Module, r.Number)).Where(g => g.Count() > 1))
            {
                foreach (var row in group)
                    row.Preview.Issues.Add(FeederSetupIssues.DuplicateSlot);
            }

            // The same part in two slots is sometimes intentional (high-usage parts), so only a warning.
            foreach (var group in rows.Where(r => r.Component != null)
                .GroupBy(r => r.Component.Id).Where(g => g.Select(r => (r.Module, r.Number)).Distinct().Count() > 1))
            {
                foreach (var row in group)
                    row.Preview.Issues.Add(FeederSetupIssues.DuplicateComponent);
            }

            foreach (var row in rows)
                row.Preview.HasError = row.Preview.Issues.Any(i => FeederSetupIssues.Errors.Contains(i));

            preview.Rows = rows.Select(r => r.Preview).ToList();
            preview.ErrorCount = preview.Rows.Count(r => r.HasError) + preview.GlobalErrors.Count;
            preview.WarningCount = preview.Rows.Count(r => !r.HasError && r.Issues.Count > 0);
            preview.NewModules = rows.Where(r => r.Preview.Issues.Contains(FeederSetupIssues.NewModule))
                .Select(r => r.Module).Distinct().ToList();
            preview.NewSlotCount = rows.Where(r => r.Preview.Issues.Contains(FeederSetupIssues.NewModule)
                    || r.Preview.Issues.Contains(FeederSetupIssues.NewSlot))
                .Select(r => (r.Module, r.Number)).Distinct().Count();
            preview.CanImport = preview.ErrorCount == 0;

            return new ImportContext { Preview = preview, Machine = machine, Rows = rows };
        }

        private FeederSetupDiff BuildDiff(FeederSetup setup, FeederSetup against)
        {
            var diff = new FeederSetupDiff
            {
                SetupId = setup.Id,
                Version = setup.Version,
                AgainstSetupId = against?.Id,
                AgainstVersion = against?.Version,
            };

            var newItems = setup.Items.ToDictionary(i => i.FeederSlotId);
            var oldItems = (against?.Items ?? new List<FeederSetupItem>()).ToDictionary(i => i.FeederSlotId);

            foreach (var slotId in newItems.Keys.Union(oldItems.Keys))
            {
                newItems.TryGetValue(slotId, out var newItem);
                oldItems.TryGetValue(slotId, out var oldItem);
                var slot = (newItem ?? oldItem).FeederSlot;

                diff.Rows.Add(new FeederSetupDiffRow
                {
                    FeederSlotId = slotId,
                    ModuleName = slot.MachineModule.Name,
                    ModuleOrder = slot.MachineModule.Order,
                    SlotNumber = slot.Number,
                    SlotCode = slot.SlotCode,
                    Change = oldItem == null ? FeederSetupDiffChange.Added
                        : newItem == null ? FeederSetupDiffChange.Removed
                        : oldItem.ComponentId != newItem.ComponentId ? FeederSetupDiffChange.Changed
                        : FeederSetupDiffChange.Unchanged,
                    OldRCode = oldItem?.Component.RCode,
                    OldSpecification = oldItem?.Component.Specification,
                    NewRCode = newItem?.Component.RCode,
                    NewSpecification = newItem?.Component.Specification,
                });
            }

            diff.Rows = diff.Rows
                .OrderBy(r => r.ModuleOrder)
                .ThenBy(r => r.ModuleName)
                .ThenBy(r => r.SlotNumber, FeederSlotNumberComparer.Instance)
                .ToList();

            return diff;
        }

        private FeederSetupResponse ToDetailResponse(FeederSetup setup)
        {
            var response = _mapper.Map<FeederSetupResponse>(setup);

            response.Items = _mapper.Map<List<FeederSetupItemResponse>>(setup.Items)
                .OrderBy(i => i.ModuleOrder)
                .ThenBy(i => i.ModuleName)
                .ThenBy(i => i.SlotNumber, FeederSlotNumberComparer.Instance)
                .ToList();

            return response;
        }

        private static bool IsBlank(FeederSetupImportRow row)
        {
            return string.IsNullOrWhiteSpace(row.Module)
                && string.IsNullOrWhiteSpace(row.Feeder)
                && string.IsNullOrWhiteSpace(row.Code);
        }

        private class ImportContext
        {
            public FeederSetupPreview Preview { get; set; }

            public LineMachine Machine { get; set; }

            public List<ResolvedRow> Rows { get; set; }
        }

        private class ResolvedRow
        {
            public string Module { get; set; }

            public string Number { get; set; }

            public string Code { get; set; }

            public Component Component { get; set; }

            public FeederSetupPreviewRow Preview { get; set; }
        }
    }
}
