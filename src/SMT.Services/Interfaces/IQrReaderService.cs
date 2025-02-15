using SMT.ViewModel.Dto.QrReaderDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Services.Interfaces
{
    public interface IQrReaderService
    {
        Task<IEnumerable<QrReaderResponse>> GetAllAsync(bool? isActive);

        Task<QrReaderResponse> GetAsync(int id);

        Task<QrReaderResponse> GetByNameAsync(string name);

        Task<QrReaderResponse> AddAsync(QrReaderCreate qrReaderCreate);

        Task<QrReaderResponse> UpdateAsync(int id, QrReaderUpdate qrReaderUpdate);

        Task<QrReaderResponse> DeleteAsync(int id);
    }
}
