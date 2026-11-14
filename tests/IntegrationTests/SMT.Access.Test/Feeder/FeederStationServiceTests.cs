using Microsoft.EntityFrameworkCore;
using SMT.Domain;
using SMT.Domain.Feeder;
using SMT.ViewModel.Dto.FeederSetupDto;
using SMT.ViewModel.Dto.FeederStationDto;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SMT.Access.Test.Feeder
{
    public class FeederStationServiceTests : IDisposable
    {
        private const string ResistorQr = "RC0402-10K@R0001@A-01@14.11.2026 10:00:00";
        private const string ResistorAlternateQr = "ERJ2-10K@R0001@A-01@14.11.2026 10:05:00";
        private const string DiodeQr = "1N4148W@D0001@A-03@14.11.2026 10:00:00";

        private readonly FeederTestContext _ctx = new FeederTestContext();

        public void Dispose()
        {
            _ctx.Dispose();
        }

        private string Slot(string module, int number) => $"FS-{_ctx.Line1.Id}-{module}-{number:00}";

        // M1-1 resistor (imported by part number), M1-2 capacitor (imported by RCode), active.
        private async Task<int> ActiveSetupAsync(params (string Module, string Feeder, string Code)[] rows)
        {
            _ctx.AddMachine(_ctx.Line1, ("M1", 3));
            var service = _ctx.FeederSetupService();

            var import = new FeederSetupImport
            {
                LineId = _ctx.Line1.Id,
                ModelId = _ctx.Model.Id,
                Side = BoardSide.Top,
                Rows = (rows.Length > 0 ? rows : new[] { ("M1", "1", "RC0402-10K"), ("M1", "2", "C0001") })
                    .Select((r, i) => new FeederSetupImportRow { RowNumber = i + 2, Module = r.Item1, Feeder = r.Item2, Code = r.Item3 })
                    .ToList(),
            };

            var setup = await service.ImportAsync(import, "op");
            await service.ActivateAsync(setup.Id, "op");

            return setup.Id;
        }

        private FeederReelScan Scan(int setupId, string reel, string slot = null) =>
            new FeederReelScan { LineId = _ctx.Line1.Id, SetupId = setupId, ReelCode = reel, SlotCode = slot };

        [Fact]
        public async Task GetStation_ListsActiveSetupsAndActiveModel()
        {
            var setupId = await ActiveSetupAsync();
            _ctx.Db.Add(new LineActiveModel { LineId = _ctx.Line1.Id, ModelId = _ctx.Model.Id, UpdatedAt = DateTime.Now });
            _ctx.Db.SaveChanges();

            var station = await _ctx.FeederStationService().GetStationAsync(_ctx.Line1.Id);

            Assert.Equal("NXT-III", station.MachineName);
            Assert.Equal(_ctx.Model.Id, station.ActiveModelId);
            var setup = Assert.Single(station.Setups);
            Assert.Equal(setupId, setup.SetupId);
            Assert.Equal(2, setup.ItemCount);
        }

        [Fact]
        public async Task Lookup_OurQr_ShowsModuleAndFeeder()
        {
            var setupId = await ActiveSetupAsync();

            var result = await _ctx.FeederStationService().LookupAsync(Scan(setupId, ResistorQr), "op");

            Assert.Equal(FeederScanResult.Ok, result.Result);
            Assert.Equal("R0001", result.RCode);
            var target = Assert.Single(result.Targets);
            Assert.Equal("M1", target.ModuleName);
            Assert.Equal("1", target.SlotNumber);
            Assert.Empty(_ctx.Db.FeederScanLogs);   // lookups are only logged when NG
        }

        [Fact]
        public async Task Lookup_AlternatePartNumber_IsOkWithWarning()
        {
            var setupId = await ActiveSetupAsync();

            var result = await _ctx.FeederStationService().LookupAsync(Scan(setupId, ResistorAlternateQr), "op");

            Assert.Equal(FeederScanResult.OkAlternate, result.Result);
            Assert.True(result.IsAlternate);
            Assert.Equal("ERJ2-10K", result.ReelPartNumber);
        }

        [Fact]
        public async Task Lookup_SetupImportedByRCode_AnyPartNumberIsNotAlternate()
        {
            var setupId = await ActiveSetupAsync(("M1", "1", "R0001"));

            var result = await _ctx.FeederStationService().LookupAsync(Scan(setupId, ResistorAlternateQr), "op");

            Assert.Equal(FeederScanResult.Ok, result.Result);
            Assert.False(result.IsAlternate);
        }

        [Fact]
        public async Task Lookup_NotInSetupAndUnknown_AreNgAndLogged()
        {
            var setupId = await ActiveSetupAsync();
            var service = _ctx.FeederStationService();

            var notInSetup = await service.LookupAsync(Scan(setupId, DiodeQr), "op");
            var unknown = await service.LookupAsync(Scan(setupId, "XYZ-999@R9999@A@1"), "op");

            Assert.Equal(FeederNgReason.NotInSetup, notInSetup.NgReason);
            Assert.Equal("D0001", notInSetup.RCode);
            Assert.Empty(notInSetup.Targets);
            Assert.Equal(FeederNgReason.UnknownReel, unknown.NgReason);
            Assert.Equal(2, _ctx.Db.FeederScanLogs.Count(l => l.Result == FeederScanResult.Ng));
        }

        [Fact]
        public async Task Verify_RightSlot_IsOkAndRecordsWhatIsLoaded()
        {
            var setupId = await ActiveSetupAsync();

            var result = await _ctx.FeederStationService().VerifyAsync(Scan(setupId, ResistorQr, Slot("M1", 1).ToLower()), "op");

            Assert.Equal(FeederScanResult.Ok, result.Result);
            Assert.Equal(Slot("M1", 1), result.ScannedSlot.SlotCode);

            var slot = await _ctx.Db.FeederSlots.SingleAsync(s => s.SlotCode == Slot("M1", 1));
            Assert.Equal(_ctx.Resistor.Id, slot.CurrentComponentId);
            Assert.Equal(ResistorQr, slot.CurrentReelCode);

            var log = await _ctx.Db.FeederScanLogs.SingleAsync();
            Assert.Equal(FeederScanResult.Ok, log.Result);
            Assert.Equal(slot.Id, log.FeederSlotId);
        }

        [Fact]
        public async Task Verify_WrongSlot_IsNgAndShowsWhatBelongsThere()
        {
            var setupId = await ActiveSetupAsync();

            var result = await _ctx.FeederStationService().VerifyAsync(Scan(setupId, ResistorQr, Slot("M1", 2)), "op");

            Assert.Equal(FeederScanResult.Ng, result.Result);
            Assert.Equal(FeederNgReason.WrongSlot, result.NgReason);
            Assert.Equal("C0001", result.ScannedSlotExpectedRCode);
            Assert.Equal("1", Assert.Single(result.Targets).SlotNumber);

            var slot = await _ctx.Db.FeederSlots.SingleAsync(s => s.SlotCode == Slot("M1", 2));
            Assert.Null(slot.CurrentComponentId);
            Assert.Equal(_ctx.Capacitor.Id, (await _ctx.Db.FeederScanLogs.SingleAsync()).ExpectedComponentId);
        }

        [Fact]
        public async Task Verify_UnknownStickerOrOtherLine_IsNg()
        {
            var setupId = await ActiveSetupAsync();
            _ctx.AddMachine(_ctx.Line2, ("M1", 1));
            var service = _ctx.FeederStationService();

            var notASticker = await service.VerifyAsync(Scan(setupId, ResistorQr, ResistorQr), "op");
            var unknownSticker = await service.VerifyAsync(Scan(setupId, ResistorQr, "FS-999-M1-01"), "op");
            var otherLine = await service.VerifyAsync(Scan(setupId, ResistorQr, $"FS-{_ctx.Line2.Id}-M1-01"), "op");

            Assert.Equal(FeederNgReason.UnknownSlot, notASticker.NgReason);
            Assert.Equal(FeederNgReason.UnknownSlot, unknownSticker.NgReason);
            Assert.Equal(FeederNgReason.WrongLine, otherLine.NgReason);
        }

        [Fact]
        public async Task Verify_AgainstReplacedSetup_IsNoActiveSetup()
        {
            var setupId = await ActiveSetupAsync();
            var setups = _ctx.FeederSetupService();
            var v2 = await setups.ImportAsync(new FeederSetupImport
            {
                LineId = _ctx.Line1.Id,
                ModelId = _ctx.Model.Id,
                Side = BoardSide.Top,
                Rows = { new FeederSetupImportRow { RowNumber = 2, Module = "M1", Feeder = "1", Code = "R0001" } },
            }, "op");
            await setups.ActivateAsync(v2.Id, "op");

            var result = await _ctx.FeederStationService().VerifyAsync(Scan(setupId, ResistorQr, Slot("M1", 1)), "op");

            Assert.Equal(FeederNgReason.NoActiveSetup, result.NgReason);
        }

        [Fact]
        public async Task Verify_SameComponentInTwoSlots_EitherIsOk_AndReelMovesBetweenThem()
        {
            var setupId = await ActiveSetupAsync(("M1", "1", "R0001"), ("M1", "3", "R0001"));
            var service = _ctx.FeederStationService();

            var lookup = await service.LookupAsync(Scan(setupId, ResistorQr), "op");
            Assert.Equal(new[] { "1", "3" }, lookup.Targets.Select(t => t.SlotNumber));

            await service.VerifyAsync(Scan(setupId, ResistorQr, Slot("M1", 1)), "op");
            var moved = await service.VerifyAsync(Scan(setupId, ResistorQr, Slot("M1", 3)), "op");

            Assert.Equal(FeederScanResult.Ok, moved.Result);
            var slots = await _ctx.Db.FeederSlots.Where(s => s.CurrentReelCode == ResistorQr).ToListAsync();
            Assert.Equal(Slot("M1", 3), Assert.Single(slots).SlotCode);
        }

        [Fact]
        public async Task GetRecentScans_NewestFirst()
        {
            var setupId = await ActiveSetupAsync();
            var service = _ctx.FeederStationService();
            await service.VerifyAsync(Scan(setupId, ResistorQr, Slot("M1", 2)), "op");
            await service.VerifyAsync(Scan(setupId, ResistorQr, Slot("M1", 1)), "op");

            var scans = (await service.GetRecentScansAsync(_ctx.Line1.Id, 10)).ToList();

            Assert.Equal(new[] { FeederScanResult.Ok, FeederScanResult.Ng }, scans.Select(s => s.Result));
            Assert.Equal("R0001", scans[0].ScannedRCode);
            Assert.Equal("M1", scans[0].ModuleName);
        }
    }
}
