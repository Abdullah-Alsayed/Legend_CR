using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace DicomApp.Helpers.Services.Telegram
{
    internal class TelegramService : ITelegramService
    {
        public async Task<bool> SendMessage(string userName, string message)
        {
            var botClient = new TelegramBotClient("");

            try
            {
                var chat = await botClient.GetChatAsync(userName);
                var result = await botClient.SendTextMessageAsync(chat.Id, message);
                Console.WriteLine("Message sent successfully!");
                return result != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
