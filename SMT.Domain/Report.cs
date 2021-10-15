using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Domain
{
    public class Report
    {
        public int Id { get; set; }

        public string Barcode { get; set; }

        public int LineId { get; set; }

        public Line Line { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public int DefectId { get; set; }

        public Defect Defect { get; set; }

        public bool Status { get; set; }

        public DateTime Date { get; set; }
    }
}
