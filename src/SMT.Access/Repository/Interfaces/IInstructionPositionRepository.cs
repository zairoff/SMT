using SMT.Access.Repository.Base;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IInstructionPositionRepository : IBaseRepository<InstructionPosition>
    {
        Task<IEnumerable<InstructionPosition>> GetByAsync(Expression<Func<InstructionPosition, bool>> expression);

        Task<IEnumerable<InstructionPosition>> GetByLineAsync(int lineId);
    }
}
