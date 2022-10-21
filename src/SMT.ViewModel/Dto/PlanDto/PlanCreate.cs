using System;

namespace SMT.ViewModel.Dto.PlanDto
{
    public class PlanCreate
    {
        public int LineId { get; set; }

        public int ModelId { get; set; }

        public int RequiredCount { get; set; }

        public int ProducedCount { get; set; }

        public DateTime Date { get; set; }
    }
}
