
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DicomApp.BL.Services;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.DAL.DB;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using DicomApp.Helpers;

namespace DicomApp.Portal.Controllers
{

    public class PaymentReceiveController : Controller
    {
        private readonly ShippingDBContext _context;
        public PaymentReceiveController(ShippingDBContext context)
        {
            _context = context;
        }

        //[HttpPost]
        //public IActionResult ListPaymentReceive([FromBody] PaymentReceiveRequest request)
        //{
        //    request.context = _context;
        //    request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
        //    request.UserID = AuthHelper.GetClaimValue(User, "UserID");
        //    var PaymentReceiveResponse = PaymentReceiveService.ListPaymentReceive(request);
        //    return Ok(new
        //    {
        //        PaymentReceiveResponse
        //    });
        //}
        //public IActionResult AccountStatement(int clientID = 0)
        //{
        //    var datasetRecords = new List<InvoiceDTO>();
        //    var invoiceRequest = new InvoiceRequest
        //    {
        //        context = _context,
        //        RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
        //        UserID = AuthHelper.GetClaimValue(User, "UserID"),
        //        IsDesc = true,
        //        InvoiceRecord=new InvoiceDTO() {VendorId= clientID }
        //    };
        //    var invoiceResponse = InvoiceService.ListInvoice(invoiceRequest);
        //    if (invoiceResponse.InvoiceRecords != null)
        //    {
        //        var paymentRequest = new PaymentReceiveRequest
        //        {
        //            context = _context,
        //            RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
        //            UserID = AuthHelper.GetClaimValue(User, "UserID"),
        //            IsDesc = true,
        //            PaymentReceiveRecord = new PaymentReceiveDTO
        //            {
        //                ClientId = clientID
        //            }
        //        };
        //        var paymentResponse = PaymentReceiveService.ListPaymentReceive(paymentRequest);
        //        if (invoiceResponse.InvoiceRecords != null)
        //            //datasetRecords = invoiceResponse.InvoiceRecords.Where(p => p.StatusId == (int)EnumStatus.Invoice_Generated).ToList();
        //        if (datasetRecords != null)
        //        {
        //            ViewBag.TotalInvoices = datasetRecords.Sum(p => p.SubTotal).ToString();
        //            ViewBag.TotalReback = datasetRecords.Sum(p => p.ReturnedAmount).ToString();
        //        }
        //        else
        //        {
        //            ViewBag.TotalInvoices = 0;
        //            ViewBag.TotalReback = 0;
        //        }

        //        if (paymentResponse.PaymentReceiveRecords != null)
        //        {
        //            ViewBag.TotalTake = paymentResponse.PaymentReceiveRecords.Where(p => p.PaymentTypeId == 1).Sum(t => t.PaymentAmount).ToString();
        //            ViewBag.TotalGive = paymentResponse.PaymentReceiveRecords.Where(p => p.PaymentTypeId == 2).Sum(t => t.PaymentAmount).ToString();

        //        }
        //        else
        //        {
        //            ViewBag.TotalTake = 0;
        //            ViewBag.TotalGive = 0;
        //        }

        //        var clientName = _context.CommonUser.FirstOrDefault(p => p.Id == clientID);

        //        if (clientName != null)
        //            ViewBag.clientName = clientName.Name;

        //        if (datasetRecords != null)
        //        {
        //            for (int i = 0; i < datasetRecords.Count(); i++)
        //            {

        //                //ÝæÇÊíÑ
        //                datasetRecords[i].InvoiceId = 5;
        //                //datasetRecords[i].TotalAmount2 = 0;
        //            }

        //        }

        //        if (paymentResponse.PaymentReceiveRecords != null)
        //        {
        //            for (int i = 0; i < paymentResponse.PaymentReceiveRecords.Count(); i++)
        //            {

        //                var record = new InvoiceDTO();
        //                record.InvoiceTypeId = 2;
        //                //record.Name = paymentResponse.PaymentReceiveRecords[i].PaymentReceiveName;
        //                record.InvoiceDueDate = paymentResponse.PaymentReceiveRecords[i].PaymentDate;
        //                if ((paymentResponse.PaymentReceiveRecords[i].TypeId == (int)EnumPurchasingType.MilanoTakeMoney && paymentResponse.PaymentReceiveRecords[i].ClientId > 0) || (paymentResponse.PaymentReceiveRecords[i].TypeId == (long)EnumPurchasingType.Add))
        //                {

        //                    if (paymentResponse.PaymentReceiveRecords[i].PaymentAmount > 0)
        //                    {//ÏÝÚÇÊ
        //                        record.InvoiceId = 1;
        //                        //record.TotalAmount = paymentResponse.PaymentReceiveRecords[i].PaymentAmount;
        //                        //record.TotalAmount2 = 0;

        //                    }
        //                    else
        //                    {//ÏÝÚÇÊ
        //                        record.InvoiceId = 1;
        //                        //record.TotalAmount2 = paymentResponse.PaymentReceiveRecords[i].PaymentAmount * (-1);

        //                        //.TotalAmount = 0;
        //                    }
        //                }
        //                else
        //                {
        //                    //ÏÝÚÇÊ
        //                    record.InvoiceId = 4;
        //                    //record.TotalAmount2 = (long)paymentResponse.PaymentReceiveRecords[i].PaymentAmount;
        //                    //record.TotalAmount = 0;
        //                }
        //                datasetRecords.Add(record);
        //            }

        //        }

        //        var data = datasetRecords.OrderBy(t => t.PaymentDate).ToArray();
        //        var currentamout = 0.00;
        //        for (int i = 0; i < data.Count(); i++)
        //        {
        //            //currentamout += (double)(data.ElementAt(i).TotalAmount2 - data.ElementAt(i).TotalAmount);
        //            data.ElementAt(i).Discount = (long)currentamout;
        //        }
        //        return View(data.ToList());
        //    }
        //    else
        //    {
        //       return View(datasetRecords);
        //    }

        //}

        //[HttpPost]
        //public IActionResult DeletePaymentReceive([FromBody] PaymentReceiveRequest request)
        //{
        //    request.context = _context;
        //    request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
        //    request.UserID = AuthHelper.GetClaimValue(User, "UserID");
        //    var PaymentReceiveResponse = PaymentReceiveService.DeletePaymentReceive(request);
        //    return Ok(new
        //    {
        //        PaymentReceiveResponse
        //    });
        //}

        //[HttpPost]
        //[AuthorizePerRole("AddPaymentReceive")]
        //public IActionResult AddPaymentReceive([FromBody] PaymentReceiveRequest request)
        //{
        //    request.context = _context;
        //    request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
        //    request.UserID = AuthHelper.GetClaimValue(User, "UserID");
        //    var PaymentReceiveResponse = PaymentReceiveService.AddPaymentReceive(request);
        //    return Ok(new
        //    {
        //        PaymentReceiveResponse
        //    });
        //}

        //[HttpPost]
        //[AuthorizePerRole("EditPaymentReceive")]
        //public IActionResult EditPaymentReceive([FromBody] PaymentReceiveRequest request)
        //{
        //    request.context = _context;
        //    request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
        //    request.UserID = AuthHelper.GetClaimValue(User, "UserID");
        //    var PaymentReceiveResponse = PaymentReceiveService.EditPaymentReceive(request);
        //    return Ok(new
        //    {
        //        PaymentReceiveResponse
        //    });
        //}



        public IActionResult ListPayment()
        {
            //var paymentRequest = new PaymentReceiveRequest();
            //paymentRequest.context = _context;

            //paymentRequest.IsCashOrPolica = true;
            //paymentRequest.LoadAllDetails = true;

            //var paymentResponse = PaymentReceiveService.ListPaymentReceive(paymentRequest);
            //return View(paymentResponse.PaymentReceiveRecords);
            return View();
        }

        public IActionResult DeletePayment(int paymentID)
        {
            //var paymentRequest = new PaymentReceiveRequest();
            //paymentRequest.PaymentReceiveRecord = new PaymentReceiveDTO();
            //paymentRequest.PaymentReceiveRecord.PaymentReceiveId = paymentID;
            //var paymentResponse = PaymentReceiveService.DeletePaymentReceive(paymentRequest);
            //if (paymentResponse.Success.HasValue && paymentResponse.Success.Value)
            //    SessionHelper.Set("SuccessMessage", ResourceManager.UIGetResource("PaymentDeletedSuccessfuly"));
            //else
            //    SessionHelper.Set("FailMessage", paymentResponse.Message);
            return null;
        }

        public IActionResult AddPaymentGiveCustomer()
        {

            //FillAddPaymentFields(null, 1);
            return View();
        }
        public IActionResult AddPaymentGiveOther()
        {

            FillAddUserFields(1);
            return View();
        }
        public IActionResult AddPaymentGiveSourcer()
        {

            //FillAddPaymentFields(null, 2);
            return View();
        }
        public IActionResult AddPaymentTakeCustomer()
        {

            //FillAddPaymentFields(null, 1);
            return View();
        }
        public IActionResult AddPaymentTakeOther()
        {
            FillAddUserFields(2);
            return View();
        }
        public IActionResult AddPaymentTakeSourcer()
        {
            //FillAddPaymentFields(null, 2);
            return View();
        }

        public IActionResult AddPaymentDiscount()
        {

            //FillAddPaymentFields();
            return View();
        }
        public IActionResult AddPaymentAdd()
        {

            //FillAddPaymentFields();
            return View();
        }
        public IActionResult AddPaymentDiscountMaterial()
        {
            FillAddUserFields(3);
            //FillAddPaymentFields(null, 2);
            return View();
        }
        public IActionResult AddPaymentAddMaterial()
        {
            FillAddUserFields(3);
            //FillAddPaymentFields(null, 2);
            return View();
        }

        //[HttpPost]
        //public IActionResult AddPayment(PaymentReceiveDTO model)
        //{
        //    var paymentRequest = new PaymentReceiveRequest();
        //    paymentRequest.PaymentReceiveRecord = model;
        //    paymentRequest.context = _context;
        //    //int code = GetPaymentCode(model.PaymentTypeId, model.ClientType, model.ClientID == null ? true : false);
        //    //model.pay = code;
        //    var paymentResponse = PaymentReceiveService.AddPaymentReceive(paymentRequest);
        //    if (paymentResponse.Success == true)
        //    {
        //        if (!model.IsPrint)
        //        {
        //            //SessionHelper.Set("SuccessMessage", ResourceManager.UIGetResource("PaymentAddedSuccessfuly"));
        //            return RedirectToAction("ListPayment");

        //        }
        //        else
        //            //return DetailsReport(invoices: int.Parse(paymentRequest.PaymentReceiveRecord.PaymentReceiveId.ToString()), isPDf: true);
        //            return RedirectToAction("ListPayment");
        //    }
        //    else
        //    {
        //        //SessionHelper.Set("FailMessage", paymentResponse.Message);
        //        return RedirectToAction("ListPayment");
        //    }

        //}



        //private void FillAddPaymentFields(PaymentReceiveDTO paymentRecord = null, long clientTypeID = 0)
        //{
        //    var userRequest = new UserRequest
        //    {
        //        context = _context,
        //    };
        //    userRequest.UserDTO = new UserDTO();
        //    userRequest.UserDTO.RoleID = 4;
        //    var userResponse = UserService.GetAllUsers(userRequest);
        //    ViewBag.client = userResponse.UserDTOs.Select(p => new { ID = p.Id, Name = p.Name }).ToArray();

        //    var purchasingTypeResponse = _context.PurchasingType.ToList();
        //    ViewBag.purchasingType = purchasingTypeResponse;

        //    //var paymentTypeRequest = new PaymentTypeRequest { InnerCall = true, AuthToken = User.Identity.Name };
        //    //var paymentTypeResponse = paymentTypeService.DoPaymentTypeList(paymentTypeRequest);
        //    //ViewBag.paymentType = paymentTypeResponse.PaymentTypeRecords;

        //}

        //public IActionResult DetailsReport(int invoices, bool isPDf = true)
        //{
        //    string reportType = "PDF";
        //    string filename = "";
        //    string LogoText = "";
        //    string Phones = "";
        //    var app = ApplicationManager.GetApplicationEntityFromRouteData();
        //    if (app != null)
        //    {
        //        LogoText = app.Name_LS;
        //        Phones = app.Notes_LS;
        //    }

        //    LocalReport localReport = new LocalReport();
        //    localReport.ReportPath = Server.MapPath("~/ReportPayment.rdlc");

        //    var paymentService = new PaymentService();
        //    var paymentRequest = new PaymentRequest
        //    {
        //        InnerCall = true,
        //        AuthToken = User.Identity.Name,
        //        Filter = new PaymentRequestFilter
        //        {
        //            ID = invoices
        //        }
        //    };
        //    var paymentResponse = paymentService.DoPaymentList(paymentRequest);
        //    var paymentRecord = paymentResponse.PaymentRecords;

        //    if (isPDf)
        //    {
        //        filename = string.Format("{0}-({2}).{1}", "ÅíÕÇá", "PDF", paymentRecord[0].ClientName + "-" + paymentRecord[0].PurchsingTypeName);
        //    }
        //    else
        //    {
        //        reportType = "Excel";
        //        filename = string.Format("{0}-({2}).{1}", "ÅíÕÇá", "xls", paymentRecord[0].ClientName + "-" + paymentRecord[0].PurchsingTypeName);
        //    }
        //    ReportParameter[] parameters = new ReportParameter[3];
        //    var totalInvoice = NumberHelper.ConvertToEasternArabicString(paymentRecord[0].Quantity.ToString("N2"));
        //    parameters[0] = new ReportParameter("ClientCredit", totalInvoice);
        //    parameters[1] = new ReportParameter("LogoText", LogoText);
        //    parameters[2] = new ReportParameter("Phones", Phones);
        //    localReport.SetParameters(parameters);
        //    ReportDataSource reportDataSource = new ReportDataSource("DataSet1", paymentRecord);
        //    localReport.DataSources.Add(reportDataSource);

        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;
        //    string deviceInfo = "";



        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;

        //    //Render the report
        //    renderedBytes = localReport.Render(
        //        reportType,
        //        deviceInfo,
        //        out mimeType,
        //        out encoding,
        //        out fileNameExtension,
        //        out streams,
        //        out warnings);
        //    Response.AddHeader("content-disposition", "inline; filename=" + filename);
        //    return File(renderedBytes, mimeType);
        //}

        private int GetPaymentCode(long typeID, long? clientType, bool isOther = false)
        {

            //var paymentService = new PaymentService();
            //var paymentRequest = new PaymentRequest
            //{
            //    InnerCall = true,
            //    AuthToken = User.Identity.Name,
            //    Filter = new PaymentRequestFilter
            //    {
            //        TypeID = typeID,
            //        LoadAllDetails = true
            //    }
            //};
            //if (isOther)
            //{
            //    paymentRequest.Filter.ClientID = null;
            //}
            //else
            //{
            //    paymentRequest.Filter.ClientType = clientType ?? 0;
            //}
            //var paymentResponse = paymentService.DoPaymentList(paymentRequest);
            return 0;
        }

        public IActionResult ApprovePayment(int paymentID)
        {
            //var paymentRequest = new PaymentReceiveRequest();
            //paymentRequest.PaymentReceiveRecord = new PaymentReceiveDTO();
            //paymentRequest.PaymentReceiveRecord.PaymentReceiveId = paymentID;
            //paymentRequest.LoadAllDetails = true;

            //var paymentResponse = PaymentReceiveService.ListPaymentReceive(paymentRequest);

            //var record = paymentResponse.PaymentReceiveRecords[0];
            //record.StatusID = 1;
            //paymentRequest = new PaymentRequest
            //{
            //    InnerCall = true,
            //    AuthToken = User.Identity.Name,
            //    PaymentRecord = record,
            //};
            //var paymentResponse = paymentService.DoPaymentUpdate(paymentRequest);
            //if (paymentResponse.Success.HasValue && paymentResponse.Success.Value)
            //{
            //    SessionHelper.Set("SuccessMessage", ResourceManager.UIGetResource("PaymentEditedSuccessfuly"));

            //    return RedirectToAction("ListPayment");
            //}
            //else
            //{
            //    SessionHelper.Set("FailMessage", paymentResponse.Message);
            //}

            return RedirectToAction("ListPayment");
        }
        public IActionResult RejectPayment(long paymentID)
        {

            //var paymentService = new PaymentService();
            //var paymentRequest = new PaymentRequest
            //{
            //    InnerCall = true,
            //    AuthToken = User.Identity.Name,
            //    Filter = new PaymentRequestFilter
            //    {
            //        ID = paymentID,
            //        LoadAllDetails = true
            //    }
            //};

            //var paymentResponseList = paymentService.DoPaymentList(paymentRequest);


            //var record = paymentResponseList.PaymentRecords[0];
            //record.StatusID = 2;
            //paymentRequest = new PaymentRequest
            //{
            //    InnerCall = true,
            //    AuthToken = User.Identity.Name,
            //    PaymentRecord = record,
            //};
            //var paymentResponse = paymentService.DoPaymentUpdate(paymentRequest);
            //if (paymentResponse.Success.HasValue && paymentResponse.Success.Value)
            //{
            //    SessionHelper.Set("SuccessMessage", ResourceManager.UIGetResource("PaymentEditedSuccessfuly"));

            //    return RedirectToAction("ListPayment");
            //}
            //else
            //{
            //    SessionHelper.Set("FailMessage", paymentResponse.Message);
            //}

            return RedirectToAction("ListPayment");
        }

        private void FillAddUserFields(long otherTypeID)
        {
            //var purchasingTypeResponse = _context.PurchasingType.Where(s => s.Description == otherTypeID.ToString()).ToList();
            //ViewBag.Types = purchasingTypeResponse;
            //var userRequest = new UserRequest
            //{
            //    context = _context
            //};
            //var userResponse = UserService.GetAllUsers(userRequest);
            //ViewBag.Managers = userResponse.UserDTOs.Where(s => s.RoleID != 3 && s.RoleID != 4).ToList();


        }

        public ActionResult ListCredit()
        {
            return View();
        }

        public ActionResult ListCreditPartial(DateTime date)
        {
            //if (date == DateTime.MinValue)
            //    date = DateTime.Now;
            //ViewBag.date = date;
            //var day = date.Day;
            ////double credit = _context.PaymentReceive.Where(p => p.StatusId == (int)EnumStatus.Invoice_Generated && p.TypeId == 2).Sum(s => s.PaymentAmount);
            //double credit =0;

            //var paymentTake = 0.00;
            //var paymentGive = 0.00;
            //var paymentTakeToday = 0.00;
            //var paymentGiveToday = 0.00;
            //var paymentRequest = new PaymentReceiveRequest();
            //paymentRequest.context = _context;
            //var paymentResponse = PaymentReceiveService.ListPaymentReceive(paymentRequest);
            //if (paymentResponse.PaymentReceiveRecords != null)
            //{
            //    var paymentResponseInlast = paymentResponse.PaymentReceiveRecords.Where(p => p.PaymentDate.Date < date.Date && p.TypeId != (long)EnumPurchasingType.Add && p.TypeId != (long)EnumPurchasingType.Discount).ToArray();
            //    var paymentResponseToday = paymentResponse.PaymentReceiveRecords.Where(p => p.PaymentDate.Date == date.Date && p.TypeId != (long)EnumPurchasingType.Add && p.TypeId != (long)EnumPurchasingType.Discount).ToArray();
            //    //ViewBag.Invoices = invoiceResponse.InvoiceRecords;
            //    for (int i = 0; i < paymentResponseInlast.Count(); i++)
            //    {
            //        if (paymentResponseInlast[i].TypeId == (int)EnumPurchasingType.MilanoGiveMoney)
            //        {
            //            paymentGive += paymentResponseInlast[i].PaymentAmount;
            //        }
            //        if (paymentResponseInlast[i].TypeId == (int)EnumPurchasingType.MilanoTakeMoney)
            //        {
            //            paymentTake += paymentResponseInlast[i].PaymentAmount;
            //        }

            //    }
            //    for (int i = 0; i < paymentResponseToday.Count(); i++)
            //    {
            //        if (paymentResponseToday[i].TypeId == (int)EnumPurchasingType.MilanoGiveMoney)
            //        {
            //            paymentGiveToday += paymentResponseToday[i].PaymentAmount;
            //        }
            //        if (paymentResponseToday[i].TypeId == (int)EnumPurchasingType.MilanoTakeMoney)
            //        {
            //            paymentTakeToday += paymentResponseToday[i].PaymentAmount;
            //        }

            //    }

            //    ViewBag.TakeCredit = paymentTakeToday;
            //    ViewBag.GiveCredit = paymentGiveToday;
            //    ViewBag.Credit = (credit + paymentTake) - paymentGive;
            //    ViewBag.EndCredit = (((credit + paymentTake) - paymentGive) + paymentTakeToday) - paymentGiveToday;
            //    return View(paymentResponseToday);

            //}
            //ViewBag.TakeCredit = paymentTakeToday;
            //ViewBag.GiveCredit = paymentGiveToday;
            //ViewBag.Credit = (credit + paymentTake) - paymentGive;
            //ViewBag.EndCredit = (((credit + paymentTake) - paymentGive) + paymentTakeToday) - paymentGiveToday;

            //var totalInvoices = 0;

            ////for (int i = 0; i < invoiceResponse.InvoiceRecords.Count(); i++)
            ////{
            ////    totalInvoices += int.Parse(Math.Floor(invoiceResponse.InvoiceRecords[i]._amount).ToString());
            ////}
            return View();
        }


    }
}