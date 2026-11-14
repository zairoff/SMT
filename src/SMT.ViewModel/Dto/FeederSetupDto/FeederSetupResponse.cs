using SMT.Domain.Feeder;
using System;
using System.Collections.Generic;

namespace SMT.ViewModel.Dto.FeederSetupDto
{
    public class FeederSetupResponse
    {
        public int Id { get; set; }

        public int ModelId { get; set; }

        public string ModelName { get; set; }

        public int LineId { get; set; }

        public string LineName { get; set; }

        public BoardSide Side { get; set; }

        public int Version { get; set; }

        public FeederSetupStatus Status { get; set; }

        public string SourceFileName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ActivatedBy { get; set; }

        public DateTime? ActivatedAt { get; set; }

        public int ItemCount { get; set; }

        // Filled only for single-setup requests, empty in lists.
        public List<FeederSetupItemResponse> Items { get; set; } = new List<FeederSetupItemResponse>();
    }

    public class FeederSetupItemResponse
    {
        public int Id { get; set; }

        public int FeederSlotId { get; set; }

        public string ModuleName { get; set; }

        public int ModuleOrder { get; set; }

        public string SlotNumber { get; set; }

        public string SlotCode { get; set; }

        public int ComponentId { get; set; }

        public string RCode { get; set; }

        public List<string> PartNumbers { get; set; }

        public string Specification { get; set; }

        public string ImportedCode { get; set; }
    }
}
