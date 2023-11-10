using SMT.Domain;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace SMT.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly long _chatId;
        private readonly long _repairChatId;
        private readonly long _readyProductChatId;

        public NotificationService(ITelegramBotClient botClient, long chatId, long repairChatId, long readyProductChatId)
        {
            _botClient = botClient;
            _chatId = chatId;
            _repairChatId = repairChatId;
            _readyProductChatId = readyProductChatId;
        }

        public async Task NotifyPcbAsync(List<PcbReport> reports)
        {
            await _botClient.SendTextMessageAsync(chatId: _chatId,
                            text: $"Diqqat!!!\nXudud: {reports[0].Line.Name}\nModel: {reports[0].Model.Name}\nNuqson: {GetDefects(reports)}\nYig'uvchi: {reports[0].Employee.FullName}\nSoni: {reports.Count}");
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

        public async Task NotifyAsync(MemoryStream memoryStream, string title)
        {
            await _botClient.SendPhotoAsync(
                    chatId: _readyProductChatId,
                    photo: InputFile.FromStream(memoryStream),
                    caption: title);
        }
    }
}
