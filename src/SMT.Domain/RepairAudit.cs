using System;

namespace SMT.Domain
{
    public class RepairAudit
    {
        public int Id { get; set; }

        public string Barcode { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public string Employee { get; set; }

        public DateTime FirstScannedDate { get; set; } = DateTime.Now;

        public DateTime LastConfirmedDate { get; set; } = DateTime.Now;
    }
}
