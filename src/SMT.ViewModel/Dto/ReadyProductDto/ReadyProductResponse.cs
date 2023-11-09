using SMT.ViewModel.Dto.ModelDto;

namespace SMT.ViewModel.Dto.ReadyProductDto
{
    public class ReadyProductResponse
    {
        public int Id { get; set; }

        public int ModelId { get; set; }

        public ModelResponse Model { get; set; }

        public int Count { get; set; }
    }
}
