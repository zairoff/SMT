namespace SMT.ViewModel.Dto.ComponentDto
{
    public static class ComponentImportStatus
    {
        public const string Created = "Created";
        public const string Updated = "Updated";
        public const string Failed = "Failed";
    }

    public class ComponentBulkImportResult
    {
        public int Row { get; set; }

        public string PartNumber { get; set; }

        public string RCode { get; set; }

        public string Status { get; set; }

        public string Message { get; set; }
    }
}
