using System.Threading.Tasks;
using PayPal.Api;

namespace DicomApp.Helpers.Services.PayPal
{
    public interface IPayPalService
    {
        Task<Payment> ExecutePaymentAsync(string payerID, string paymentID);
        Task<Payment> PurchaseAccountAsync(int amount, string returnUrl, string cancelUrl);
    }
}
