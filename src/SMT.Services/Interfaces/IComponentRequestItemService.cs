using SMT.ViewModel.Dto.ComponentRequestDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IComponentRequestItemService
    {
        Task<ComponentRequestItemResponse> MarkNotFoundAsync(int id);

        Task<IEnumerable<ComponentRequestItemResponse>> TransferAsync(ComponentRequestItemTransfer transfer);
    }
}
