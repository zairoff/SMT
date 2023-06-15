﻿using SMT.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SMT.Notification
{
    public interface INotificationService
    {
        Task NotifyAsync(List<Report> reports);
        Task NotifyRepairAsync(MachineRepair repair);
    }
}
