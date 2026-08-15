using SMT.ViewModel.Dto.BoardV2Dto;
using System.Collections.Generic;

namespace SMT.ViewModel.Models.BoardFlowV2
{
    public class FlaggedBoardsResponse
    {
        public int TotalCount { get; set; }

        public List<BoardV2Response> Items { get; set; }
    }
}
