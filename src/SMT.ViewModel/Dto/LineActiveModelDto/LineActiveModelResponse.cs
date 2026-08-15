using System;

namespace SMT.ViewModel.Dto.LineActiveModelDto
{
    public class LineActiveModelResponse
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public string LineName { get; set; }

        public int ModelId { get; set; }

        public string ModelName { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
