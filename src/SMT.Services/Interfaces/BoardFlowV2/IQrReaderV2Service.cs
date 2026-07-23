using SMT.ViewModel.Dto.QrReaderV2Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces.BoardFlowV2
{
    public interface IQrReaderV2Service
    {
        Task<QrReaderV2Response> AddAsync(QrReaderV2Create create);
        Task<QrReaderV2Response> UpdateAsync(int id, QrReaderV2Update update);
        Task<QrReaderV2Response> DeleteAsync(int id);
        Task<QrReaderV2Response> GetAsync(int id);
        Task<IEnumerable<QrReaderV2Response>> GetAllAsync(int? lineId, bool? isActive);
    }
}
