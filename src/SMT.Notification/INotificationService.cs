using SMT.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Notification
{
    public interface INotificationService
    {
        Task NotifyPcbAsync(List<PcbReport> reports);
        Task NotifyRepairAsync(MachineRepair repair);
    }
}
