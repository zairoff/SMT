using SMT.Domain.Feeder;
using System.Collections.Generic;

namespace SMT.ViewModel.Dto.FeederSetupDto
{
    // Issue codes are stable strings (translated by the UI):
    //   errors   - MissingValue, UnknownCode, AmbiguousCode, DuplicateSlot, InactiveSlot
    //   warnings - DuplicateComponent, NewModule, NewSlot
    //   global   - NoMachine, NoRows
    public static class FeederSetupIssues
    {
        public const string MissingValue = "MissingValue";
        public const string UnknownCode = "UnknownCode";
        public const string AmbiguousCode = "AmbiguousCode";
        public const string DuplicateSlot = "DuplicateSlot";
        public const string InactiveSlot = "InactiveSlot";
        public const string DuplicateComponent = "DuplicateComponent";
        public const string NewModule = "NewModule";
        public const string NewSlot = "NewSlot";
        public const string NoMachine = "NoMachine";
        public const string NoRows = "NoRows";

        public static readonly HashSet<string> Errors = new HashSet<string>
        {
            MissingValue, UnknownCode, AmbiguousCode, DuplicateSlot, InactiveSlot,
        };
    }

    public class FeederSetupPreview
    {
        public int LineId { get; set; }

        public int ModelId { get; set; }

        public BoardSide Side { get; set; }

        public int NextVersion { get; set; }

        public int? ActiveVersion { get; set; }

        public List<string> GlobalErrors { get; set; } = new List<string>();

        public int ErrorCount { get; set; }

        public int WarningCount { get; set; }

        public List<string> NewModules { get; set; } = new List<string>();

        public int NewSlotCount { get; set; }

        public bool CanImport { get; set; }

        public List<FeederSetupPreviewRow> Rows { get; set; } = new List<FeederSetupPreviewRow>();
    }

    public class FeederSetupPreviewRow
    {
        public int RowNumber { get; set; }

        public string Module { get; set; }

        public string Feeder { get; set; }

        public string Code { get; set; }

        public string SlotCode { get; set; }

        public int? ComponentId { get; set; }

        public string RCode { get; set; }

        public string Specification { get; set; }

        public bool HasError { get; set; }

        public List<string> Issues { get; set; } = new List<string>();
    }
}
