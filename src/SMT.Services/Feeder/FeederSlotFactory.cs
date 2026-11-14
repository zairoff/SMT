using SMT.Access.Repository.Feeder;
using SMT.Domain.Feeder;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Feeder
{
    // Creates slots with a unique sticker code. Codes are never reused: if a module was renamed and a
    // new module took its old name, the generated code gets a "-2", "-3"... suffix instead of
    // colliding with a sticker that may still be on the machine.
    public class FeederSlotFactory
    {
        private readonly IFeederSlotRepository _slotRepository;
        private readonly HashSet<string> _reserved = new HashSet<string>();

        public FeederSlotFactory(IFeederSlotRepository slotRepository)
        {
            _slotRepository = slotRepository;
        }

        public async Task<FeederSlot> CreateAsync(int lineId, MachineModule module, string number)
        {
            var normalizedNumber = FeederCodes.NormalizeSlotNumber(number);
            var baseCode = FeederCodes.BuildSlotCode(lineId, module.Name, normalizedNumber);
            var code = baseCode;

            for (var suffix = 2; _reserved.Contains(code) || await _slotRepository.SlotCodeExistsAsync(code); suffix++)
                code = $"{baseCode}-{suffix}";

            _reserved.Add(code);

            var slot = new FeederSlot
            {
                MachineModule = module,
                Number = normalizedNumber,
                SlotCode = code,
                IsActive = true,
            };

            module.Slots.Add(slot);

            return slot;
        }
    }
}
