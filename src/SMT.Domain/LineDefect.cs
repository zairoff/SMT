﻿namespace SMT.Domain
{
    public class LineDefect
    {
        public int Id { get; set; }

        public int LineId { get; set; }

        public virtual Line Line { get; set; }

        public int DefectId { get; set; }

        public virtual Defect Defect { get; set; }
    }
}
