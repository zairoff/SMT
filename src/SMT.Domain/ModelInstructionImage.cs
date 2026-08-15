namespace SMT.Domain
{
    // The reference image shown at a given InstructionPosition when a given Model is running.
    public class ModelInstructionImage
    {
        public int Id { get; set; }

        public int ModelId { get; set; }

        public virtual Model Model { get; set; }

        public int InstructionPositionId { get; set; }

        public virtual InstructionPosition InstructionPosition { get; set; }

        public string ImagePath { get; set; }
    }
}
