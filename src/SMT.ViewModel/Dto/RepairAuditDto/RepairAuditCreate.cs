using SMT.Domain;

namespace SMT.ViewModel.Dto.RepairAuditDto
{
    public class RepairAuditCreate
    {
        public string Barcode { get; set; }

        public string Employee { get; set; }

        public RepairAuditType Type { get; set; } = RepairAuditType.Audit;
    }
}
