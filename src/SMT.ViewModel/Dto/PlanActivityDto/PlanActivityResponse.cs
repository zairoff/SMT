using SMT.ViewModel.Dto.LineDto;
using System;

namespace SMT.ViewModel.Dto.PlanActivityDto
{
    public class PlanActivityResponse
    {
        public int Id { get; set; }

        public LineResponse Line { get; set; }

        public string Issue { get; set; }

        public string Reason { get; set; }

        public string Act { get; set; }

        public string Responsible { get; set; }

        public string Status { get; set; }

        public string Shift { get; set; }

        public DateTime Date { get; set; }

        public DateTime Expires { get; set; }
    }
}
