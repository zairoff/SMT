using System;
using System.Collections.Generic;

namespace SMT.Domain
{
    public class ComponentRequest
    {
        public int Id { get; set; }

        public int? LineId { get; set; }

        public Line Line { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public List<ComponentRequestItem> Items { get; set; }
    }
}
