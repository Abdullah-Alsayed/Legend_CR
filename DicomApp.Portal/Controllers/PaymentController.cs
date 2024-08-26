using System;
using System.Threading.Tasks;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.CashDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Helpers.Services.PayPal;
using ECommerce.Core.Services.MailServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DicomApp.Portal.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ShippingDBContext _context;
        readonly IPayPalService _payPalService;
        readonly IConfiguration _configuration;
        readonly IMailServices _mailServices;

        public PaymentController(
            ShippingDBContext context,
            IPayPalService payPalService,
            IConfiguration configuration,
            IMailServices mailServices
        )
        {
            _context = context;
            _payPalService = payPalService;
            _configuration = configuration;
            _mailServices = mailServices;
        }

        [HttpPost]
        public async Task<IActionResult> Paypal(int accountId, int gameChargeId)
        {
            try
            {
                var RoleID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.RoleID);
                var UserID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.UserID);
                if (accountId != 0)
                    return await PayAcount(accountId, RoleID, UserID);
                else
                    return Json(new { success = false, message = "field" });

                //    return await Charge(gameChargeId, RoleID, UserID);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> SuccessPay(string payerID, string paymentID, int accountId)
        {
            var result = await _payPalService.ExecutePaymentAsync(payerID, paymentID);
            if (result.state != "failed")
            {
                var RoleID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.RoleID);
                var UserID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.UserID);
                var advertisementRequest = new AdvertisementRequest
                {
                    RoleID = RoleID,
                    UserID = UserID,
                    context = _context,
                    AdsDTO = new CommonDefinitions.DTO.AdvertisementDTOs.AdsDTO
                    {
                        AdvertisementId = accountId,
                        StatusType = (int)StatusTypeEnum.Sold
                    },
                    applyFilter = true,
                };
                var accountResponse =
                    DicomApp.BL.Services.AdvertisementService.ChangStatusAdvertisement(
                        advertisementRequest
                    );
                if (accountResponse.Success)
                {
                    var transaction = TransactionService.SuccessPay(
                        new TransactionRequest
                        {
                            context = _context,
                            UserID = UserID,
                            RoleID = RoleID,
                            TransactionDTO = new TransactionDTO
                            {
                                PaymentId = paymentID,
                                AdvertisementId = accountId
                            }
                        }
                    );

                    var body =
                        $"userName : {accountResponse.AdsDTO.UserName} ,  password : {accountResponse.AdsDTO.Password}";
                    var emailResult = await _mailServices.SendAsync(
                        new EmailDto
                        {
                            Email = accountResponse.AdsDTO.Gamer.Email,
                            Subject = "congratulations",
                            Body = body
                        }
                    );
                }
                return View();
            }
            else
                return RedirectToAction("CancelPay");
        }

        public async Task<IActionResult> CancelPay()
        {
            return View();
        }

        #region helpers
        private async Task<IActionResult> PayAcount(int accountId, int RoleID, int UserID)
        {
            var advertisementRequest = new AdvertisementRequest
            {
                RoleID = RoleID,
                UserID = UserID,
                context = _context,
                AdsDTO = new AdsDTO { AdvertisementId = accountId },
                applyFilter = true,
            };
            var accountResponse = DicomApp.BL.Services.AdvertisementService.GetAdvertisement(
                advertisementRequest
            );
            if (accountResponse.Success && accountResponse.AdsDTO != null)
            {
                var account = accountResponse.AdsDTO;
                var returnUrl =
                    $"{_configuration["AppDomain"]}/Payment/SuccessPay?AccountId={accountId}";
                var cancelUrl = $"{_configuration["AppDomain"]}/Payment/CancelPay";
                var createPayment = await _payPalService.PurchaseAccountAsync(
                    account.Price,
                    returnUrl,
                    cancelUrl
                );
                var approverUrl = createPayment
                    ?.links?.Find(x => x.rel.ToLower() == "approval_url")
                    ?.href;

                if (!string.IsNullOrEmpty(approverUrl))
                {
                    TransactionService.AddTransaction(
                        new TransactionRequest
                        {
                            UserID = UserID,
                            RoleID = RoleID,
                            context = _context,
                            TransactionDTO = new TransactionDTO
                            {
                                AdvertisementId = accountId,
                                PaymentId = createPayment.id,
                                Amount = account.Price,
                                BuyerId = UserID
                            }
                        }
                    );
                    return Json(new { success = true, message = approverUrl });
                }
                else
                    return Json(new { success = false, message = "field" });
            }
            else
                return Json(new { success = false, message = "field" });
        }

        //private async Task<IActionResult> Charge(int gameChargeId, int RoleID, int UserID)
        //{
        //    var gameChargeRequest = new GameChargeRequest
        //    {
        //        RoleID = RoleID,
        //        UserID = UserID,
        //        context = _context,
        //        GameChargeDTO = new GameChargeDTO { Id = gameChargeId },
        //        applyFilter = true,
        //    };
        //    var gameChargeResponse = DicomApp.BL.Services.GameChargeService.GetGameCharge(
        //        gameChargeRequest
        //    );
        //    if (gameChargeResponse.Success && gameChargeResponse.GameChargeDTO != null)
        //    {
        //        var gameCharge = gameChargeResponse.GameChargeDTO;
        //        var returnUrl =
        //            $"{_configuration["AppDomain"]}/Payment/SuccessPay?serviceId={gamerServiceResponse.ServiceDTO.GamerServiceId}";
        //        var cancelUrl = $"{_configuration["AppDomain"]}/Payment/CancelPay";
        //        var createPayment = await _payPalService.PurchaseAccountAsync(
        //            gameCharge.Price - gameCharge.Discount,
        //            returnUrl,
        //            cancelUrl
        //        );
        //        var approverUrl = createPayment
        //            ?.links?.Find(x => x.rel.ToLower() == "approval_url")
        //            ?.href;

        //        if (!string.IsNullOrEmpty(approverUrl))
        //        {
        //            TransactionService.AddTransaction(
        //                new TransactionRequest
        //                {
        //                    UserID = UserID,
        //                    RoleID = RoleID,
        //                    context = _context,
        //                    TransactionDTO = new TransactionDTO
        //                    {
        //                        AdvertisementId = accountId,
        //                        PaymentId = createPayment.id,
        //                        Amount = account.Price,
        //                        BuyerId = UserID
        //                    }
        //                }
        //            );
        //            return Json(new { success = true, message = approverUrl });
        //        }
        //        else
        //            return Json(new { success = false, message = "field" });
        //    }
        //}

        #endregion
    }
}
