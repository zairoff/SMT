using SMT.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace SMT.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly long _chatId;
        private readonly long _repairChatId;

        public NotificationService(ITelegramBotClient botClient, long chatId, long repairChatId)
        {
            _botClient = botClient;
            _chatId = chatId;
            _repairChatId = repairChatId;
        }

        public async Task NotifyPcbAsync(List<PcbReport> reports)
        {
            
            await _botClient.SendTextMessageAsync(chatId: _chatId,
                            text: $"Diqqat!!!\nXudud: {reports[0].Line.Name}\nModel:{reports[0].Model.Name}\nNuqson: {GetDefects(reports)}\nYig'uvchi: {reports[0].Employee.FullName}\nSoni: {reports.Count}");
        }

        private static string GetDefects(List<PcbReport> reports)
        {
            var defects = new HashSet<string>();
            foreach(var report in reports)
                defects.Add(report.Defect.Name);

            return string.Join(", ", defects);
        }

        public async Task NotifyRepairAsync(MachineRepair repair)
        {
            await _botClient.SendTextMessageAsync(chatId: _repairChatId,
                                text: $"Diqqat!!!\nUskuna:" +
                                $" {repair.Machine.Name}\nSabab:" +
                                $" {repair.Issue}\nBajarildi:" +
                                $" {repair.Action}\nBajardi:" +
                                $" {repair.Employee.FullName}\nGacha amal qiladi: " +
                                $"{repair.NotificationDate}");
        }

        public async Task NotifyAsync(List<Report> reports)
        {
            await _botClient.SendTextMessageAsync(chatId: _chatId,
                            text: $"Diqqat!!!\nXudud: {reports[0].Line.Name}\nModel:{reports[0].Model.Name}\nNuqson: {reports[0].Defect.Name}\nSoni: {reports.Count}");
        }
    }
}
