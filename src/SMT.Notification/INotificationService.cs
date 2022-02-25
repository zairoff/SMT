using SMT.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Notification
{
    public interface INotificationService
    {
        Task Notify(List<PcbReport> reports, int count);
    }
}
