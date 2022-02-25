using System;
using System.ComponentModel.DataAnnotations;

namespace SMT.ViewModel.Dto.PcbReportDto
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
