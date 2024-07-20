using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PayPal.Api;

namespace DicomApp.Helpers.Services.PayPal
{
    public class PayPalService : IPayPalService
    {
        readonly IConfiguration configuration;
        readonly string _accessToken;

        public PayPalService(IConfiguration configuration)
        {
            this.configuration = configuration;
            var clientId = configuration["PayPal:ClientId"];
            var clientSecret = configuration["PayPal:ClientSecret"];
            var mode = configuration["PayPal:Mode"];

            var config = new Dictionary<string, string>
            {
                { "mode", mode },
                { "clientId", clientId },
                { "clientSecret", clientSecret }
            };

            _accessToken = new OAuthTokenCredential(
                clientId,
                clientSecret,
                config
            ).GetAccessToken();
        }

        public async Task<Payment> ExecutePaymentAsync(string payerID, string paymentID)
        {
            var apiContext = new APIContext(_accessToken)
            {
                Config = ConfigManager.Instance.GetProperties()
            };
            var paymentExecution = new PaymentExecution() { payer_id = payerID, };
            var payment = new Payment() { id = paymentID };
            var executePayment = await Task.Run(
                () => payment.Execute(apiContext, paymentExecution)
            );
            return executePayment;
        }

        public async Task<Payment> PurchaseAccountAsync(
            int amount,
            string returnUrl,
            string cancelUrl
        )
        {
            var finalAmount = amount.ToString("0.00");
            var apiContext = new APIContext(_accessToken);
            var items = new ItemList
            {
                items = new List<Item>
                {
                    new Item
                    {
                        name = "Purchase new Account",
                        currency = "USD",
                        price = finalAmount,
                        quantity = "1",
                        sku = "Purchase Account"
                    }
                }
            };
            var transaction = new Transaction()
            {
                amount = new Amount
                {
                    currency = "USD",
                    total = finalAmount,
                    details = new Details { subtotal = finalAmount, },
                },
                item_list = items,
                description = "Purchase Account"
            };

            var payment = new Payment
            {
                intent = "sale",
                payer = new Payer { payment_method = "paypal" },
                redirect_urls = new RedirectUrls
                {
                    cancel_url = cancelUrl,
                    return_url = returnUrl,
                },
                transactions = new List<Transaction> { transaction }
            };
            var createPayment = payment.Create(apiContext);

            return createPayment;
        }
    }
}
