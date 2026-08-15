namespace SMT.ViewModel.Dto.ModelInstructionImageDto
{
    public class ModelInstructionImageResponse
    {
        public int Id { get; set; }

        public int ModelId { get; set; }

        public string ModelName { get; set; }

        public int InstructionPositionId { get; set; }

        public string PositionName { get; set; }

        public string ImagePath { get; set; }
    }
}
