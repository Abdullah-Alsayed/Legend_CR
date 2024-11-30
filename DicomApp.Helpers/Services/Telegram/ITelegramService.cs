using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DicomApp.Helpers.Services.Telegram
{
    public interface ITelegramService
    {
        Task<bool> SendMessage(string userName, string message);
    }
}
