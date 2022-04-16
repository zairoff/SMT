using Microsoft.EntityFrameworkCore;
using SMT.Access.Data;
using SMT.Access.Repository.Base;
using SMT.Access.Repository.Interfaces;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SMT.Access.Repository
{
    public class DefectRepository : BaseRepository<Defect>, IDefectRepository
    {
        public DefectRepository(AppDbContext context) : base(context)
        {
        }
    }
}
