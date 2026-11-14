using AutoMapper;
using SMT.Access.Repository.Feeder;
using SMT.Access.Repository.Interfaces;
using SMT.Access.Unit;
using SMT.Domain.Feeder;
using SMT.Services.Exceptions;
using SMT.Services.Interfaces.Feeder;
using SMT.ViewModel.Dto.LineMachineDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMT.Services.Feeder
{
    public class LineMachineService : ILineMachineService
    {
        // A slot range bigger than this is almost certainly a typo (e.g. 1..100 instead of 1..10).
        private const int MaxSlotsPerRequest = 200;

        private readonly ILineMachineRepository _repository;
        private readonly IMachineModuleRepository _moduleRepository;
        private readonly IFeederSlotRepository _slotRepository;
        private readonly ILineRepository _lineRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LineMachineService(ILineMachineRepository repository, IMachineModuleRepository moduleRepository,
            IFeederSlotRepository slotRepository, ILineRepository lineRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _moduleRepository = moduleRepository;
            _slotRepository = slotRepository;
            _lineRepository = lineRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<LineMachineResponse>> GetAllAsync()
        {
            var machines = await _repository.GetAllWithLayoutAsync();

            return machines.Select(ToResponse).ToList();
        }

        public async Task<LineMachineResponse> GetByLineAsync(int lineId)
        {
            var machine = await _repository.GetByLineAsync(lineId);

            return machine == null ? null : ToResponse(machine);
        }

        public async Task<LineMachineResponse> SaveAsync(LineMachineSave save)
        {
            if (string.IsNullOrWhiteSpace(save.Name))
                throw new InvalidOperationException("Machine name is required");

            if (!Enum.IsDefined(typeof(MachineVendor), save.Vendor))
                throw new InvalidOperationException($"Unknown vendor: {save.Vendor}");

            var line = await _lineRepository.FindAsync(l => l.Id == save.LineId);
            if (line == null)
                throw new NotFoundException($"Line {save.LineId} not found");

            var machine = await _repository.GetByLineAsync(save.LineId);

            if (machine == null)
            {
                machine = new LineMachine { LineId = save.LineId };
                await _repository.AddAsync(machine);
            }

            machine.Name = save.Name.Trim();
            machine.Vendor = save.Vendor;
            machine.IsActive = true;

            await _unitOfWork.SaveAsync();

            return await GetByLineAsync(save.LineId);
        }

        public async Task<LineMachineResponse> AddModuleAsync(int lineId, MachineModuleSave save)
        {
            var machine = await _repository.GetByLineAsync(lineId);
            if (machine == null)
                throw new NotFoundException($"Line {lineId} has no machine configured");

            var name = RequireModuleName(save.Name);
            if (machine.Modules.Any(m => m.Name == name))
                throw new ConflictException($"Module {name} already exists on this machine");

            machine.Modules.Add(new MachineModule { Name = name, Order = save.Order });

            await _unitOfWork.SaveAsync();

            return ToResponse(machine);
        }

        // Renaming keeps existing slot codes, so stickers already on the machine stay valid.
        public async Task<LineMachineResponse> UpdateModuleAsync(int moduleId, MachineModuleSave save)
        {
            var module = await _moduleRepository.FindAsync(m => m.Id == moduleId);
            if (module == null)
                throw new NotFoundException($"Module {moduleId} not found");

            var name = RequireModuleName(save.Name);
            var conflict = await _moduleRepository.FindAsync(m => m.LineMachineId == module.LineMachineId
                && m.Id != moduleId && m.Name == name);
            if (conflict != null)
                throw new ConflictException($"Module {name} already exists on this machine");

            module.Name = name;
            module.Order = save.Order;

            await _unitOfWork.SaveAsync();

            return await GetByLineAsync(module.LineMachine.LineId);
        }

        public async Task<LineMachineResponse> AddSlotsAsync(int moduleId, FeederSlotsCreate create)
        {
            if (create.From < 1 || create.To < create.From)
                throw new InvalidOperationException("Slot range must be From >= 1 and To >= From");

            if (create.To - create.From + 1 > MaxSlotsPerRequest)
                throw new InvalidOperationException($"At most {MaxSlotsPerRequest} slots can be added at once");

            var module = await _moduleRepository.FindAsync(m => m.Id == moduleId);
            if (module == null)
                throw new NotFoundException($"Module {moduleId} not found");

            var factory = new FeederSlotFactory(_slotRepository);

            for (var number = create.From; number <= create.To; number++)
            {
                var normalized = FeederCodes.NormalizeSlotNumber(number.ToString());
                var existing = module.Slots.FirstOrDefault(s => s.Number == normalized);

                if (existing != null)
                    existing.IsActive = true;
                else
                    await factory.CreateAsync(module.LineMachine.LineId, module, normalized);
            }

            await _unitOfWork.SaveAsync();

            return await GetByLineAsync(module.LineMachine.LineId);
        }

        // Slots are deactivated rather than deleted - setups and scan history reference them.
        public async Task<LineMachineResponse> SetSlotActiveAsync(int slotId, bool isActive)
        {
            var slot = await _slotRepository.FindAsync(s => s.Id == slotId);
            if (slot == null)
                throw new NotFoundException($"Slot {slotId} not found");

            slot.IsActive = isActive;

            await _unitOfWork.SaveAsync();

            return await GetByLineAsync(slot.MachineModule.LineMachine.LineId);
        }

        private static string RequireModuleName(string name)
        {
            var normalized = FeederCodes.NormalizeModule(name);
            if (normalized.Length == 0)
                throw new InvalidOperationException("Module name is required");

            return normalized;
        }

        private LineMachineResponse ToResponse(LineMachine machine)
        {
            var response = _mapper.Map<LineMachineResponse>(machine);

            response.Modules = response.Modules
                .OrderBy(m => m.Order)
                .ThenBy(m => m.Name)
                .ToList();

            foreach (var module in response.Modules)
                module.Slots = module.Slots.OrderBy(s => s.Number, FeederSlotNumberComparer.Instance).ToList();

            return response;
        }
    }
}
