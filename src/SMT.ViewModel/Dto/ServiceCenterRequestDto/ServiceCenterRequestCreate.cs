namespace SMT.ViewModel.Dto.ServiceCenterRequestDto
{
    public class ServiceCenterRequestCreate
    {
        public int ServiceCenterId { get; set; }

        public int RequestSenderId { get; set; }

        public int MessageId { get; set; }
    }
}
