using System.Collections.Generic;

namespace SMT.ViewModel.Dto.ComponentRequestDto
{
    public class ComponentRequestCreate
    {
        public int? LineId { get; set; }

        public List<ComponentRequestItemCreate> Items { get; set; }
    }
}
