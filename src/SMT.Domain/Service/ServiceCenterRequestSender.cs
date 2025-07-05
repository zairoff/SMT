namespace SMT.Domain.Service
{
    public class ServiceCenterRequestSender
    {
        public int Id { get; set; }

        public int ServiceCenterId { get; set; }

        public ServiceCenter ServiceCenter { get; set; }

        public long ChatId { get; set; }

        public string Phone { get; set; }
    }
}
