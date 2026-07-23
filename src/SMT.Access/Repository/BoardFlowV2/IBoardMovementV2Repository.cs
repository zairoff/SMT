using SMT.Access.Repository.Base;
using SMT.Domain.BoardFlow.V2;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Access.Repository.BoardFlowV2
{
    public interface IBoardMovementV2Repository : IBaseRepository<BoardMovementV2>
    {
        Task<IEnumerable<BoardMovementV2>> GetByBoardAsync(int boardId);

        Task<IEnumerable<BoardMovementV2>> GetByDateAsync(DateTime date);
    }
}
