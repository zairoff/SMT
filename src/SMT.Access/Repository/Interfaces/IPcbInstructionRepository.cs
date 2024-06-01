using SMT.Access.Repository.Base;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IPcbInstructionRepository : IBaseRepository<PcbInstruction>
    {
        Task<IEnumerable<PcbInstruction>> GetByAsync(Expression<Func<PcbInstruction, bool>> expression);
    }
}
