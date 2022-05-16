namespace SMT.ViewModel.Dto.RepairDto
{
    public class RepairCreate
    {
        public int ReportId { get; set; }

        public int EmployeeId { get; set; }

        public string Action { get; set; }

        public string Condition { get; set; }
    }
}
