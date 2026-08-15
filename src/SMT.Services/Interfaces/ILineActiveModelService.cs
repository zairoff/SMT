using SMT.ViewModel.Dto.LineActiveModelDto;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface ILineActiveModelService
    {
        Task<LineActiveModelResponse> GetByLineAsync(int lineId);

        Task<LineActiveModelResponse> SetActiveModelAsync(LineActiveModelSet activeModelSet);
    }
}
