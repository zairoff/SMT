using System;

namespace SMT.Domain.Feeder
{
    public class ChangeoverItem
    {
        public int Id { get; set; }

        public int ChangeoverSessionId { get; set; }

        public ChangeoverSession ChangeoverSession { get; set; }

        public int FeederSlotId { get; set; }

        public FeederSlot FeederSlot { get; set; }

        public ChangeoverAction Action { get; set; }

        // Null for Remove items - the slot just has to be emptied.
        public int? RequiredComponentId { get; set; }

        public Component RequiredComponent { get; set; }

        public bool IsVerified { get; set; }

        public DateTime? VerifiedAt { get; set; }
    }
}
