using SMT.Domain;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SMT.Notification
{
    public interface INotificationService
    {
        Task NotifyPcbAsync(List<PcbReport> reports);
        Task NotifyAsync(List<Report> reports);
        Task NotifyRepairAsync(MachineRepair repair);
        Task NotifyAsync(MemoryStream memoryStream, string title);
    }
}
