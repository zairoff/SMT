using SMT.Access.Repository.Base;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IModelInstructionImageRepository : IBaseRepository<ModelInstructionImage>
    {
        Task<IEnumerable<ModelInstructionImage>> GetByAsync(Expression<Func<ModelInstructionImage, bool>> expression);
    }
}
