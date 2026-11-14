using SMT.Domain.Feeder;
using System.Collections.Generic;

namespace SMT.ViewModel.Dto.FeederStationDto
{
    // What the operator station of a line needs to start: the line, its machine and the active
    // setups it can verify against. ActiveModelId is the line's active model, used as the default.
    public class FeederStationResponse
    {
        public int LineId { get; set; }

        public string LineName { get; set; }

        public string MachineName { get; set; }

        public int? ActiveModelId { get; set; }

        public List<FeederStationSetup> Setups { get; set; } = new List<FeederStationSetup>();
    }

    public class FeederStationSetup
    {
        public int SetupId { get; set; }

        public int ModelId { get; set; }

        public string ModelName { get; set; }

        public BoardSide Side { get; set; }

        public int Version { get; set; }

        public int ItemCount { get; set; }
    }
}
