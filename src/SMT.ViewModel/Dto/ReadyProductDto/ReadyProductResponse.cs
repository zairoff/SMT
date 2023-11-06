using SMT.ViewModel.Dto.ModelDto;
using System;

namespace SMT.ViewModel.Dto.ReadyProductDto
{
    public class ReadyProductResponse
    {
        public int Id { get; set; }

        public ModelResponse Model { get; set; }

        public int Count { get; set; }

        public bool Inside { get; set; }

        public DateTime Enter { get; set; }

        public DateTime Exit { get; set; }
    }
}
