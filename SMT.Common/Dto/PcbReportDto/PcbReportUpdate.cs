using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Common.Dto.PcbReportDto
{
    public class PcbReportUpdate
    {
        [Required]
        public int DefectId { get; set; }

        [Required]
        public int PcbPositionId { get; set; }
    }
}
