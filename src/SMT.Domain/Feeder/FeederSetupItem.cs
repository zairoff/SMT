namespace SMT.Domain.Feeder
{
    public class FeederSetupItem
    {
        public int Id { get; set; }

        public int FeederSetupId { get; set; }

        public FeederSetup FeederSetup { get; set; }

        public int FeederSlotId { get; set; }

        public FeederSlot FeederSlot { get; set; }

        public int ComponentId { get; set; }

        public Component Component { get; set; }

        // The code as written in the imported Excel (part number or RCode). A reel with a different
        // part number but the same RCode is accepted as an alternate.
        public string ImportedCode { get; set; }
    }
}
