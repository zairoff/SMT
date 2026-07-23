using SMT.ViewModel.Dto.BoardMovementV2Dto;
using SMT.ViewModel.Dto.BoardV2Dto;
using System.Collections.Generic;

namespace SMT.ViewModel.Models.BoardFlowV2
{
    public class BoardHistoryResponse
    {
        public BoardV2Response Board { get; set; }

        public List<BoardMovementV2Response> Movements { get; set; } = new List<BoardMovementV2Response>();
    }
}
