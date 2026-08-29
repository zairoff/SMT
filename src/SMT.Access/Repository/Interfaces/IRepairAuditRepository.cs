using SMT.Access.Repository.Base;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository.Interfaces
{
    public interface IRepairAuditRepository : IBaseRepository<RepairAudit>
    {
        public Task<RepairAudit> FindByBarcodeAsync(string barcode);

        public Task<IEnumerable<RepairAudit>> GetByAsync(Expression<Func<RepairAudit, bool>> expression);
    }
}
