using System.Collections.Generic;

namespace SMT.ViewModel.Dto.ComponentRequestDto
{
    public class ComponentRequestResponse
    {
        public int Id { get; set; }

        public int? LineId { get; set; }

        public string LineName { get; set; }

        public string CreatedDate { get; set; }

        public List<ComponentRequestItemResponse> Items { get; set; }
    }
}
