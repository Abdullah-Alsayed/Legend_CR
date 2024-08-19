using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.Helpers.Services.PayPal;
using DicomApp.Helpers.Services.Telegram;

namespace DicomApp.CommonDefinitions.Requests
{
    public class GamerServiceRequest : BaseRequest
    {
        public string RoutPath { get; set; }
        public ServiceDTO ServiceDTO { get; set; } = new ServiceDTO();
        public IPayPalService PaymentService { get; set; }
        public ITelegramService TelegramService { get; set; }
    }
}
