using System;

namespace SMT.Domain.Service
{
    public class ServiceCenterRequest
    {
        public int Id { get; set; }

        public int MessageId { get; set; }

        public int ServiceCenterId { get; set; }

        public int RequestSenderId { get; set; }

        public ServiceCenterRequestSender RequestSender { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
