using System;

namespace SMT.Domain.Service
{
    public class ServiceCenterResult
    {
        public int Id { get; set; }

        public int ServiceCenterRequestId { get; set; }

        public ServiceCenterRequest ServiceCenterRequest { get; set; }

        public int ServiceCenterRepairerId { get; set; }

        public ServiceCenterRepairer ServiceCenterRepairer { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        public ServiceStatus ServiceStatus { get; set; }
    }
}

