using SMT.Domain.BoardFlow.V2;

namespace SMT.Access.Repository.BoardFlowV2
{
    // Query-projection row, not a persisted entity - used to build a live
    // per-station snapshot from BoardV2's current state.
    public class BoardStationCount
    {
        public int ReaderId { get; set; }

        public BoardStatusV2 Status { get; set; }

        public int Count { get; set; }
    }
}
