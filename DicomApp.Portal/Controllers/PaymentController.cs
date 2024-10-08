using System;
using System.Threading.Tasks;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.CashDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.Helpers.Services.PayPal;
using ECommerce.Core.Services.MailServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace DicomApp.Portal.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ShippingDBContext _context;
        readonly IPayPalService _payPalService;
        readonly IConfiguration _configuration;
        readonly IMailServices _mailServices;
        readonly IStringLocalizer<PaymentController> _stringLocalizer;
        private readonly IHostingEnvironment _hosting;

        public PaymentController(
            ShippingDBContext context,
            IPayPalService payPalService,
            IConfiguration configuration,
            IMailServices mailServices,
            IHostingEnvironment hosting,
            IStringLocalizer<PaymentController> stringLocalizer
        )
        {
            _context = context;
            _hosting = hosting;
            _payPalService = payPalService;
            _configuration = configuration;
            _mailServices = mailServices;
            _stringLocalizer = stringLocalizer;
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

        public async Task<IActionResult> PayFromAttatchment(int accountId, IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    var RoleID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.RoleID);
                    var UserID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.UserID);
                    if (accountId != 0)
                    {
                        var account = AdvertisementService.GetAdvertisement(
                            new AdvertisementRequest
                            {
                                context = _context,
                                AdsDTO = new AdsDTO { AdvertisementId = accountId }
                            }
                        );
                        var img = BaseHelper.UploadImg(file, _hosting.WebRootPath);
                        var transactionResult = TransactionService.AddTransaction(
                            new TransactionRequest
                            {
                                context = _context,
                                UserID = UserID,
                                RoleID = RoleID,
                                TransactionDTO = new TransactionDTO
                                {
                                    AdvertisementId = accountId,
                                    TransactionType = TransactionTypeEnum.Account,
                                    TransactionSource = TransactionSourceEnum.Attachments,
                                    Attachment = img,
                                    BuyerId = UserID,
                                    Amount = account?.AdsDTO?.Price ?? 0
                                }
                            }
                        );

                        var result = AdvertisementService.ChangStatusAdvertisement(
                            new AdvertisementRequest
                            {
                                context = _context,
                                UserID = UserID,
                                RoleID = RoleID,
                                AdsDTO = new AdsDTO
                                {
                                    AdvertisementId = accountId,
                                    StatusType = (int)StatusTypeEnum.Pending
                                }
                            }
                        );
                        return Json(result);
                    }
                    else
                        return Json(new { success = false, message = "field" });
                }
                else
                    return Json(new { success = false, message = "field" });
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

        public async Task<IActionResult> AcceptPayment(int Id)
        {
            var transactionRequest = new TransactionRequest
            {
                RoleID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.RoleID),
                UserID = AuthHelper.GetClaimValue(User, SystemConstants.Claims.UserID),
                context = _context,
                TransactionDTO = new TransactionDTO { TransactionId = Id },
                applyFilter = true,
            };
            var transactionResponse = DicomApp.BL.Services.TransactionService.AcceptPayment(
                transactionRequest
            );
            if (transactionResponse.Success)
            {
                var body =
                    $"userName : {transactionResponse.TransactionDTO.Advertisement.UserName} ,  password : {transactionResponse.TransactionDTO.Advertisement.Password}";
                var emailResult = await _mailServices.SendAsync(
                    new EmailDto
                    {
                        Email = transactionResponse.TransactionDTO.Advertisement.Gamer.Email,
                        Subject = "congratulations",
                        Body = body
                    }
                );
                return Json(
                    new
                    {
                        success = true,
                        message = _stringLocalizer[SystemConstants.Message.Success].ToString()
                    }
                );
            }
            else
                return Json(
                    new
                    {
                        success = false,
                        message = _stringLocalizer[SystemConstants.Message.Field].ToString()
                    }
                );
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
                                TransactionSource = TransactionSourceEnum.Paypal,
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
