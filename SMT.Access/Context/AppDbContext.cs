using Microsoft.EntityFrameworkCore;
using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Access.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Defect> Defects { get; set; }
    }
}
