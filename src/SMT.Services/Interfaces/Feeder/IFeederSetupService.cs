using SMT.Domain.Feeder;
using SMT.ViewModel.Dto.FeederSetupDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces.Feeder
{
    public interface IFeederSetupService
    {
        Task<IEnumerable<FeederSetupResponse>> GetAllAsync(int? lineId, int? modelId, FeederSetupStatus? status);
        Task<FeederSetupResponse> GetAsync(int id);
        Task<FeederSetupPreview> PreviewAsync(FeederSetupImport import);
        Task<FeederSetupResponse> ImportAsync(FeederSetupImport import, string username);
        Task<FeederSetupResponse> ActivateAsync(int id, string username);
        Task<FeederSetupDiff> DiffAsync(int id, int? againstId);
        Task<FeederSetupResponse> CopyToLineAsync(int id, FeederSetupCopy copy, string username);
        Task<FeederSetupResponse> DeleteAsync(int id);
    }
}
