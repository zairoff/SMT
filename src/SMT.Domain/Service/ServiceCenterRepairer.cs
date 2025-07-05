namespace SMT.Domain.Service
{
    public class ServiceCenterRepairer
    {
        public int Id { get; set; }

        public long TelegramId { get; set; }

        public long ChatId { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }
    }
}
