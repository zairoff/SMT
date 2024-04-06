using SMT.ViewModel.Dto.LineDto;
using SMT.ViewModel.Dto.ModelDto;
using System;

namespace SMT.ViewModel.Dto.HourlyPlanDto
{
    public class HourlyPlanResponse
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public LineResponse Line { get; set; }

        public int ModelId { get; set; }

        public ModelResponse Model { get; set; }

        public int Plan { get; set; }

        public int Produced { get; set; }

        public DateTime Time { get; set; }
    }
}
