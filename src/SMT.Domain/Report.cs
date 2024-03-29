﻿using System;

namespace SMT.Domain
{
    public class Report
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public Line Line { get; set; }

        public int DefectId { get; set; }

        public Defect Defect { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }

        public bool Status { get; set; }

        public string Barcode { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
