using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain
{
    public class PcbReport
    {
        public int Id { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int DefectId { get; set; }

        public Defect Defect { get; set; }

        public int PcbId { get; set; }

        public PcbPosition Pcb { get; set; }

        public DateTime Date { get; set; }
    }
}
