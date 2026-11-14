namespace SMT.ViewModel.Dto.ReturnedProductScanDto
{
    public class ReturnedProductScanResponse
    {
        public int Id { get; set; }

        public string Barcode { get; set; }

        public int? ModelId { get; set; }

        public string ModelName { get; set; }

        public string SapCode { get; set; }

        public string SerialCode { get; set; }

        public int Count { get; set; }

        public string Date { get; set; }
    }
}
