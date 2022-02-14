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

        public NotificationService(ITelegramBotClient botClient, long chatId)
        {
            _botClient = botClient;
            _chatId = chatId;
        }

        public async Task Notify(List<PcbReport> reports, int count)
        {
            
            await _botClient.SendTextMessageAsync(chatId: _chatId,
                            text: "Diqqat!!!\nXudud: PCBA-1\nModel: " + 
                            reports[0].Model.Name + "\nNuqson: " +
                            GetDefects(reports) + "\nYig'uvchi: " + 
                            reports[0].PositionId + "\nSoni: " + count);
        }

        private static string GetDefects(List<PcbReport> reports)
        {
            var defects = new HashSet<string>();
            foreach(var report in reports)
                defects.Add(report.Defect.Name);

            return string.Join(", ", defects);
        }
    }
}
