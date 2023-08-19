using SMT.Access.Repository.Base;
using SMT.Domain;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace SMT.Access.Repository.Interfaces
{
    public interface ILineOwnerRepository : IBaseRepository<LineOwner>
    {
        Task<IEnumerable<LineOwner>> GetByAsync(Expression<Func<LineOwner, bool>> expression);
    }
}
