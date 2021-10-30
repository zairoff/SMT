﻿using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain
{
    public class Department
    {
        public int Id { get; set; }

        public HierarchyId HierarchyId { get; set; }

        public string Name { get; set; }
    }
}
