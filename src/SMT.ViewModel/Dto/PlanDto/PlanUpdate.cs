using System;

namespace SMT.ViewModel.Dto.PlanDto
{
    public class PlanUpdate
    {
        public int LineId { get; set; }

        public int ModelId { get; set; }

        public int RequiredCount { get; set; }

        public int ProducedCount { get; set; }

        public string Employee { get; set; }

        public string DayNight { get; set; }

        public DateTime Date { get; set; }
    }
}
