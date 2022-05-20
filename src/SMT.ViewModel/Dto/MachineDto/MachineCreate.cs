using Microsoft.AspNetCore.Http;

namespace SMT.ViewModel.Dto.MachineDto
{
    public class MachineCreate
    {
        public string Name { get; set; }

        public IFormFile File { get; set; }
    }
}
