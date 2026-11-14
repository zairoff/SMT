using System.Collections.Generic;

namespace SMT.ViewModel.Dto.FeederSetupDto
{
    public enum FeederSetupDiffChange
    {
        Unchanged = 0,
        Added = 1,
        Removed = 2,
        Changed = 3,
    }

    public class FeederSetupDiff
    {
        public int SetupId { get; set; }

        public int Version { get; set; }

        // Null when there's nothing to compare with (first version) - every row is then Added.
        public int? AgainstSetupId { get; set; }

        public int? AgainstVersion { get; set; }

        public List<FeederSetupDiffRow> Rows { get; set; } = new List<FeederSetupDiffRow>();
    }

    public class FeederSetupDiffRow
    {
        public int FeederSlotId { get; set; }

        public string ModuleName { get; set; }

        public int ModuleOrder { get; set; }

        public string SlotNumber { get; set; }

        public string SlotCode { get; set; }

        public FeederSetupDiffChange Change { get; set; }

        public string OldRCode { get; set; }

        public string OldSpecification { get; set; }

        public string NewRCode { get; set; }

        public string NewSpecification { get; set; }
    }
}
