using System.ComponentModel.DataAnnotations;

namespace SMT.ViewModel.Dto.PcbReportDto
{
    public class PcbReportUpdate
    {
        [Required]
        public int DefectId { get; set; }

        [Required]
        public int PcbPositionId { get; set; }
    }
}
