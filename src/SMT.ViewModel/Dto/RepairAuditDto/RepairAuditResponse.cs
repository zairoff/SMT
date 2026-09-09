namespace SMT.ViewModel.Dto.RepairAuditDto
{
    public class RepairAuditResponse
    {
        public int Id { get; set; }

        public string Barcode { get; set; }

        public string Employee { get; set; }

        public string Type { get; set; }

        public string ModelName { get; set; }

        public string SapCode { get; set; }

        public string FirstScannedDate { get; set; }

        public string LastConfirmedDate { get; set; }

        public bool Reconfirmed { get; set; }
    }
}
