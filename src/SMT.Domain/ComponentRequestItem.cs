using System;

namespace SMT.Domain
{
    public class ComponentRequestItem
    {
        public int Id { get; set; }

        public int ComponentRequestId { get; set; }

        public ComponentRequest ComponentRequest { get; set; }

        public int ComponentId { get; set; }

        public Component Component { get; set; }

        public ComponentRequestItemStatus Status { get; set; } = ComponentRequestItemStatus.Requested;

        public DateTime? TransferredDate { get; set; }
    }
}
