using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace DicomApp.Helpers.Services.Telegram
{
    public class TelegramService : ITelegramService
    {
        readonly IConfiguration _configuration;

        public TelegramService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendMessage(string userName, string message)
        {
            var key = _configuration["telegram"];
            var botClient = new TelegramBotClient(key);
            try
            {
                //var result = await botClient.Contacts_ResolveUsername("USERNAME");
                //var us = "LegendRC_bot (7529769946)";
                //var xxxx = await botClient.GetMeAsync();
                //var zzzzz = await botClient.GetMyNameAsync();

                var me = await botClient.GetMeAsync();
                var updates = await botClient.GetUpdatesAsync(3);
                foreach (var update in updates)
                {
                    // send response to incoming message
                    await botClient.SendTextMessageAsync(
                        update.Message.Chat.Id,
                        "The Matrix has you..."
                    );
                }
                var chat = await botClient.GetChatAsync($"@{userName}");
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
