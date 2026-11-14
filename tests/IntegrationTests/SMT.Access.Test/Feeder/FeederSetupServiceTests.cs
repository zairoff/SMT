using Microsoft.EntityFrameworkCore;
using SMT.Domain.Feeder;
using SMT.Services.Exceptions;
using SMT.ViewModel.Dto.FeederSetupDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SMT.Access.Test.Feeder
{
    public class FeederSetupServiceTests : IDisposable
    {
        private readonly FeederTestContext _ctx = new FeederTestContext();

        public void Dispose()
        {
            _ctx.Dispose();
        }

        private FeederSetupImport Import(params (string Module, string Feeder, string Code)[] rows)
        {
            return new FeederSetupImport
            {
                LineId = _ctx.Line1.Id,
                ModelId = _ctx.Model.Id,
                Side = BoardSide.Top,
                SourceFileName = "mb55.xlsx",
                Rows = rows.Select((r, i) => new FeederSetupImportRow
                {
                    RowNumber = i + 2,
                    Module = r.Module,
                    Feeder = r.Feeder,
                    Code = r.Code,
                }).ToList(),
            };
        }

        [Fact]
        public async Task Preview_ValidRows_ResolvesPartNumbersRCodesAndQr()
        {
            _ctx.AddMachine(_ctx.Line1, ("M1", 3));

            var preview = await _ctx.FeederSetupService().PreviewAsync(Import(
                ("M1", "1", "RC0402-10K"),
                ("m1", "02", "C0001"),
                ("M1", "3", "1N4148W@D0001@A-01@14.11.2026")));

            Assert.True(preview.CanImport);
            Assert.Equal(0, preview.ErrorCount);
            Assert.Equal(0, preview.WarningCount);
            Assert.Equal(1, preview.NextVersion);
            Assert.Equal(new[] { "R0001", "C0001", "D0001" }, preview.Rows.Select(r => r.RCode));
            Assert.Equal($"FS-{_ctx.Line1.Id}-M1-02", preview.Rows[1].SlotCode);
        }

        [Fact]
        public async Task Preview_AlternatePartNumber_ResolvesToSameComponent()
        {
            _ctx.AddMachine(_ctx.Line1, ("M1", 1));

            var preview = await _ctx.FeederSetupService().PreviewAsync(Import(("M1", "1", "ERJ2-10K")));

            Assert.True(preview.CanImport);
            Assert.Equal(_ctx.Resistor.Id, preview.Rows.Single().ComponentId);
        }

        [Fact]
        public async Task Preview_FlagsErrorsAndWarnings()
        {
            _ctx.AddMachine(_ctx.Line1, ("M1", 3));

            var preview = await _ctx.FeederSetupService().PreviewAsync(Import(
                ("M1", "1", "NOT-A-PART"),
                ("M1", "2", "C0001"),
                ("M1", "02", "D0001"),
                ("M1", "", "R0001"),
                ("M2", "1", "R0001"),
                ("M2", "2", "R0001"),
                ("", "", "")));

            Assert.False(preview.CanImport);
            Assert.Equal(6, preview.Rows.Count);   // the blank row is skipped
            Assert.Contains(FeederSetupIssues.UnknownCode, preview.Rows[0].Issues);
            Assert.Contains(FeederSetupIssues.DuplicateSlot, preview.Rows[1].Issues);
            Assert.Contains(FeederSetupIssues.DuplicateSlot, preview.Rows[2].Issues);
            Assert.Contains(FeederSetupIssues.MissingValue, preview.Rows[3].Issues);
            Assert.Contains(FeederSetupIssues.NewModule, preview.Rows[4].Issues);
            Assert.Contains(FeederSetupIssues.DuplicateComponent, preview.Rows[4].Issues);
            Assert.False(preview.Rows[4].HasError);
            Assert.Equal(4, preview.ErrorCount);
            Assert.Equal(2, preview.WarningCount);
            Assert.Equal(new[] { "M2" }, preview.NewModules);
            Assert.Equal(2, preview.NewSlotCount);
        }

        [Fact]
        public async Task Preview_LineWithoutMachine_IsGlobalError()
        {
            var preview = await _ctx.FeederSetupService().PreviewAsync(Import(("M1", "1", "R0001")));

            Assert.False(preview.CanImport);
            Assert.Contains(FeederSetupIssues.NoMachine, preview.GlobalErrors);
        }

        [Fact]
        public async Task Preview_InactiveSlot_IsError()
        {
            var machine = _ctx.AddMachine(_ctx.Line1, ("M1", 1));
            machine.Modules[0].Slots[0].IsActive = false;
            _ctx.Db.SaveChanges();

            var preview = await _ctx.FeederSetupService().PreviewAsync(Import(("M1", "1", "R0001")));

            Assert.Contains(FeederSetupIssues.InactiveSlot, preview.Rows.Single().Issues);
            Assert.False(preview.CanImport);
        }

        [Fact]
        public async Task Import_CreatesDraftAndMissingModulesAndSlots()
        {
            _ctx.AddMachine(_ctx.Line1, ("M1", 1));

            var setup = await _ctx.FeederSetupService().ImportAsync(Import(
                ("M1", "1", "R0001"),
                ("M3", "7", "C0001")), "operator1");

            Assert.Equal(FeederSetupStatus.Draft, setup.Status);
            Assert.Equal(1, setup.Version);
            Assert.Equal("operator1", setup.CreatedBy);
            Assert.Equal(2, setup.Items.Count);
            Assert.Equal($"FS-{_ctx.Line1.Id}-M3-07", setup.Items[1].SlotCode);

            var module = await _ctx.Db.MachineModules.Include(m => m.Slots).SingleAsync(m => m.Name == "M3");
            Assert.Equal(2, module.Order);
            Assert.Equal("7", module.Slots.Single().Number);
        }

        [Fact]
        public async Task Import_WithErrors_Throws()
        {
            _ctx.AddMachine(_ctx.Line1, ("M1", 1));

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _ctx.FeederSetupService().ImportAsync(Import(("M1", "1", "NOT-A-PART")), "operator1"));

            Assert.Empty(_ctx.Db.FeederSetups);
        }

        [Fact]
        public async Task Activate_ArchivesPreviousActiveVersion()
        {
            _ctx.AddMachine(_ctx.Line1, ("M1", 2));
            var service = _ctx.FeederSetupService();

            var v1 = await service.ImportAsync(Import(("M1", "1", "R0001")), "op");
            await service.ActivateAsync(v1.Id, "engineer");
            var v2 = await service.ImportAsync(Import(("M1", "1", "C0001")), "op");

            Assert.Equal(2, v2.Version);

            var activated = await service.ActivateAsync(v2.Id, "engineer");

            Assert.Equal(FeederSetupStatus.Active, activated.Status);
            Assert.Equal("engineer", activated.ActivatedBy);
            Assert.Equal(FeederSetupStatus.Archived, (await service.GetAsync(v1.Id)).Status);
            await Assert.ThrowsAsync<ConflictException>(() => service.ActivateAsync(v2.Id, "engineer"));

            // Rolling back to an archived version is allowed.
            await service.ActivateAsync(v1.Id, "engineer");
            Assert.Equal(FeederSetupStatus.Archived, (await service.GetAsync(v2.Id)).Status);
        }

        [Fact]
        public async Task Diff_AgainstActiveVersion()
        {
            _ctx.AddMachine(_ctx.Line1, ("M1", 3));
            var service = _ctx.FeederSetupService();

            var v1 = await service.ImportAsync(Import(("M1", "1", "R0001"), ("M1", "2", "C0001")), "op");
            await service.ActivateAsync(v1.Id, "engineer");
            var v2 = await service.ImportAsync(Import(("M1", "1", "R0001"), ("M1", "2", "D0001"), ("M1", "3", "C0001")), "op");

            var diff = await service.DiffAsync(v2.Id, null);

            Assert.Equal(v1.Id, diff.AgainstSetupId);
            Assert.Equal(
                new[] { FeederSetupDiffChange.Unchanged, FeederSetupDiffChange.Changed, FeederSetupDiffChange.Added },
                diff.Rows.Select(r => r.Change));
            Assert.Equal("C0001", diff.Rows[1].OldRCode);
            Assert.Equal("D0001", diff.Rows[1].NewRCode);

            // The active version compares with the version before it.
            var activeDiff = await service.DiffAsync(v1.Id, null);
            Assert.Null(activeDiff.AgainstSetupId);
            Assert.All(activeDiff.Rows, r => Assert.Equal(FeederSetupDiffChange.Added, r.Change));
        }

        [Fact]
        public async Task CopyToLine_CreatesDraftOnTargetMachine()
        {
            _ctx.AddMachine(_ctx.Line1, ("M1", 2));
            _ctx.AddMachine(_ctx.Line2, ("M1", 1));
            var service = _ctx.FeederSetupService();

            var source = await service.ImportAsync(Import(("M1", "1", "RC0402-10K"), ("M1", "2", "C0001")), "op");

            var copy = await service.CopyToLineAsync(source.Id, new FeederSetupCopy { TargetLineId = _ctx.Line2.Id }, "op2");

            Assert.Equal(_ctx.Line2.Id, copy.LineId);
            Assert.Equal(FeederSetupStatus.Draft, copy.Status);
            Assert.Equal(new[] { "R0001", "C0001" }, copy.Items.Select(i => i.RCode));
            Assert.Equal($"FS-{_ctx.Line2.Id}-M1-02", copy.Items[1].SlotCode);   // created on the target
        }

        [Fact]
        public async Task Delete_OnlyDrafts()
        {
            _ctx.AddMachine(_ctx.Line1, ("M1", 1));
            var service = _ctx.FeederSetupService();

            var draft = await service.ImportAsync(Import(("M1", "1", "R0001")), "op");
            var active = await service.ImportAsync(Import(("M1", "1", "C0001")), "op");
            await service.ActivateAsync(active.Id, "engineer");

            await service.DeleteAsync(draft.Id);
            await Assert.ThrowsAsync<ConflictException>(() => service.DeleteAsync(active.Id));

            Assert.Equal(new List<int> { active.Id }, _ctx.Db.FeederSetups.Select(s => s.Id).ToList());
            Assert.Single(_ctx.Db.FeederSetupItems);
        }
    }
}
