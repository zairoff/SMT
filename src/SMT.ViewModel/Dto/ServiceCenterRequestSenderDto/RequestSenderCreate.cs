namespace SMT.ViewModel.Dto.ServiceCenterRequestSenderDto
{
    public class RequestSenderCreate
    {
        public int ServiceCenterId { get; set; }

        public long ChatId { get; set; }

        public string Phone { get; set; }
    }
}
