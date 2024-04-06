using SMT.ViewModel.Dto.LineDto;
using SMT.ViewModel.Dto.ModelDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.ViewModel.Dto.HourlyPlanDto
{
    public class HourlyPlanUpdate
    {
        public int LineId { get; set; }

        public int ModelId { get; set; }

        public int Plan { get; set; }

        public int Produced { get; set; }
    }
}
