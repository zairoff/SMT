using SMT.ViewModel.Dto.LineDto;
using SMT.ViewModel.Dto.ModelDto;
using System;

namespace SMT.ViewModel.Dto.PlanDto
{
    public class PlanResponse
    {
        public int Id { get; set; }

        public int RequiredCount { get; set; }

        public int ProducedCount { get; set; }

        public string Employee { get; set; }

        public string DayNight { get; set; }

        public string Date { get; set; }

        public virtual LineResponse Line { get; set; }

        public virtual ModelResponse Model { get; set; }
    }
}
