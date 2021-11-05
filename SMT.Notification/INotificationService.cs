using SMT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Notification
{
    public interface INotificationService
    {
        Task Notify(List<PcbReport> reports, int count);
    }
}
