using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain
{
    public class Department
    {
        public int Id { get; set; }

        [Column(TypeName = "Ltree")]
        public string Ltree { get; set; }

        public string Name { get; set; }
    }
}
