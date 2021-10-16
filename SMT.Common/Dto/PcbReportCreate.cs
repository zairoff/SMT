﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Common.Dto
{
    public class PcbReportCreate
    {
        [Required]
        public int ModelId { get; set; }

        [Required]
        public int DefectId { get; set; }

        [Required]
        public int PcbPositionId { get; set; }

        [Required]
        public static DateTime Date => DateTime.UtcNow;
    }
}
