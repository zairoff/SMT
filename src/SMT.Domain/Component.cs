using System.Collections.Generic;

namespace SMT.Domain
{
    public class Component
    {
        public int Id { get; set; }

        public List<string> PartNumber { get; set; }

        public string RCode { get; set; }

        public string StorePlaceNumber { get; set; }

        public string SapPlace { get; set; }

        public string PlaceCode { get; set; }

        public string Specification { get; set; }

        public bool IsActive { get; set; }
    }
}
