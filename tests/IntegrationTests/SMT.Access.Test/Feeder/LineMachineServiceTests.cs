using SMT.Domain.Feeder;
using SMT.Services.Exceptions;
using SMT.ViewModel.Dto.LineMachineDto;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SMT.Access.Test.Feeder
{
    public class LineMachineServiceTests : IDisposable
    {
        private readonly FeederTestContext _ctx = new FeederTestContext();

        public void Dispose()
        {
            _ctx.Dispose();
        }

        [Fact]
        public async Task Save_CreatesThenUpdatesTheLinesSingleMachine()
        {
            var service = _ctx.LineMachineService();

            var created = await service.SaveAsync(new LineMachineSave { LineId = _ctx.Line1.Id, Name = "NXT", Vendor = MachineVendor.Fuji });
            var updated = await service.SaveAsync(new LineMachineSave
            {
                LineId = _ctx.Line1.Id,
                Name = "NPM-W2",
                Vendor = MachineVendor.Panasonic,
                RequireSlotScan = true,
            });

            Assert.Equal(created.Id, updated.Id);
            Assert.False(created.RequireSlotScan);
            Assert.Equal(MachineVendor.Panasonic, updated.Vendor);
            Assert.True(updated.RequireSlotScan);
            Assert.Single(_ctx.Db.LineMachines);
        }

        [Fact]
        public async Task AddSlots_SkipsExistingAndReactivates()
        {
            var service = _ctx.LineMachineService();
            await service.SaveAsync(new LineMachineSave { LineId = _ctx.Line1.Id, Name = "NXT", Vendor = MachineVendor.Fuji });
            var machine = await service.AddModuleAsync(_ctx.Line1.Id, new MachineModuleSave { Name = "m1", Order = 1 });
            var moduleId = machine.Modules.Single().Id;

            await service.AddSlotsAsync(moduleId, new FeederSlotsCreate { From = 1, To = 3 });
            var slot2 = _ctx.Db.FeederSlots.Single(s => s.Number == "2");
            await service.SetSlotActiveAsync(slot2.Id, false);

            var result = await service.AddSlotsAsync(moduleId, new FeederSlotsCreate { From = 2, To = 10 });

            var slots = result.Modules.Single().Slots;
            Assert.Equal(10, slots.Count);
            Assert.All(slots, s => Assert.True(s.IsActive));
            Assert.Equal(Enumerable.Range(1, 10).Select(n => n.ToString()), slots.Select(s => s.Number));
            Assert.Equal($"FS-{_ctx.Line1.Id}-M1-01", slots[0].SlotCode);
        }

        [Fact]
        public async Task RenamedModule_KeepsCodes_AndNewModuleWithOldNameGetsSuffixedCodes()
        {
            var service = _ctx.LineMachineService();
            await service.SaveAsync(new LineMachineSave { LineId = _ctx.Line1.Id, Name = "NXT", Vendor = MachineVendor.Fuji });
            var machine = await service.AddModuleAsync(_ctx.Line1.Id, new MachineModuleSave { Name = "M1", Order = 1 });
            var firstId = machine.Modules.Single().Id;
            await service.AddSlotsAsync(firstId, new FeederSlotsCreate { From = 1, To = 1 });

            await service.UpdateModuleAsync(firstId, new MachineModuleSave { Name = "M9", Order = 1 });
            machine = await service.AddModuleAsync(_ctx.Line1.Id, new MachineModuleSave { Name = "M1", Order = 2 });
            var secondId = machine.Modules.Single(m => m.Name == "M1").Id;
            machine = await service.AddSlotsAsync(secondId, new FeederSlotsCreate { From = 1, To = 1 });

            var lineId = _ctx.Line1.Id;
            Assert.Equal($"FS-{lineId}-M1-01", machine.Modules.Single(m => m.Name == "M9").Slots.Single().SlotCode);
            Assert.Equal($"FS-{lineId}-M1-01-2", machine.Modules.Single(m => m.Name == "M1").Slots.Single().SlotCode);
        }

        [Fact]
        public async Task AddModule_DuplicateName_Conflicts()
        {
            var service = _ctx.LineMachineService();
            await service.SaveAsync(new LineMachineSave { LineId = _ctx.Line1.Id, Name = "NXT", Vendor = MachineVendor.Fuji });
            await service.AddModuleAsync(_ctx.Line1.Id, new MachineModuleSave { Name = "M1" });

            await Assert.ThrowsAsync<ConflictException>(() =>
                service.AddModuleAsync(_ctx.Line1.Id, new MachineModuleSave { Name = " m 1" }));
        }

        [Fact]
        public async Task AddSlots_InvalidRange_Throws()
        {
            var service = _ctx.LineMachineService();

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.AddSlotsAsync(1, new FeederSlotsCreate { From = 5, To = 1 }));
        }
    }
}
