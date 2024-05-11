//using LegendCR.BL.Services;
//using LegendCR.CommonDefinitions.DTO;
//using LegendCR.CommonDefinitions.DTO.ShipmentDTOs;
//using LegendCR.CommonDefinitions.DTO.VendorProductDTOs;
//using LegendCR.CommonDefinitions.Requests;
//using LegendCR.DAL.DB;
//using LegendCR.Helpers;
//using LegendCR.Portal.Helpers;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Rotativa.AspNetCore;
//using Rotativa.AspNetCore.Options;
////using iText.Html2pdf;
////using iText.IO.Source;
////using iText.Kernel.Geom;
////using iText.Kernel.Pdf;
////using System.IO;
////using System.Text;

//namespace LegendCR.Portal.Controllers
//{
//    public class ShipmentController : Controller
//    {
//        private readonly ApplicationDBContext _context;

//        public ShipmentController(ApplicationDBContext context)
//        {
//            _context = context;
//        }

//        #region Shipment details popup

//        [AuthorizePerRole("ShipmentTrack")]
//        public ActionResult TrackShipment(string TrackShipment)
//        {
//            var RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//            var UserID = AuthHelper.GetClaimValue(User, "UserID");

//            var model = new ShipDTO();
//            model.RefId = TrackShipment;
//            if (RoleID == (int)EnumRole.Vendor)
//                model.VendorDetailsDTO = new VendorDetailsDTO { VendorId = UserID };

//            var request = new ShipmentRequest
//            {
//                RoleID = RoleID,
//                UserID = UserID,
//                context = _context,
//                ShipDTO = model,

//                HasSettingDTO = true,
//                HasCustomerDetailsDTO = true,
//                HasVendorDetailsDTO = true,
//                HasFeesDetailsDTO = true,
//                HasFollowUpDTOs = true,
//                HasShipItemDTOs = true,
//            };

//            var response = LegendCR.BL.Services.ShipmentService.GetShipment(request);

//            return PartialView("~/Views/Shared/Shipment/_MainDetails.cshtml", response.ShipDTO);
//        }

//        [AllowAnonymous]
//        public IActionResult ShipmentDetails(int id)
//        {
//            var model = new ShipDTO();
//            model.ShipmentId = id;
//            var request = new ShipmentRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ShipDTO = model,

//                HasSettingDTO = true,
//                HasCustomerDetailsDTO = true,
//                HasVendorDetailsDTO = true,
//                HasFeesDetailsDTO = true,
//                HasFollowUpDTOs = true,
//                HasShipItemDTOs = true,
//            };

//            var response = LegendCR.BL.Services.ShipmentService.GetShipment(request);

//            return PartialView("~/Views/Shared/Shipment/_MainDetails.cshtml", response.ShipDTO);
//        }

//        [AllowAnonymous]
//        public IActionResult StockItems(int id)
//        {
//            var model = new ShipItemDTO();
//            model.ShipmentId = id;
//            model.IsStcok = true;
//            var request = new ShipmentRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ShipItemDTO = model
//            };
//            var response = LegendCR.BL.Services.ShipmentService.GetShipmentItems(request);
//            return PartialView("~/Views/Shared/Shipment/_ItemsDetails.cshtml", response.ShipItemDTOs);
//        }

//        [AllowAnonymous]
//        public ActionResult PartialItems(int Id)
//        {
//            var model = new ShipItemDTO();
//            model.ShipmentId = Id;
//            model.IsStcok = false;
//            var request = new ShipmentRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ShipItemDTO = model
//            };
//            var response = LegendCR.BL.Services.ShipmentService.GetShipmentItems(request);
//            return PartialView("~/Views/Shared/Shipment/_ItemsDetails.cshtml", response.ShipItemDTOs);
//        }

//        #endregion

//        #region Shipment actions

//        [AuthorizePerRole("ShipmentAdd")]
//        [HttpPost]
//        public IActionResult Add(ShipDTO model, string PartialItems)
//        {
//            #region Data Validations

//            if (model.FeesDetailsDTO.Total <= 0)
//                return Json("COD can't be negative value");

//            if (model.SettingDTO.IsPartialDelivery)
//            {
//                if ((string.IsNullOrEmpty(model.PartialItems) || model.PartialItems == "[]") && !model.SettingDTO.IsStock)
//                    return Json("Please select shipment items");
//                else if (model.SettingDTO.IsStock)
//                {
//                    bool hasStockItems = false;
//                    var selectedStockIDs = model.ProductIDs.Split(',').Select(int.Parse).ToList();
//                    var selectedStockQuantities = model.ProductsQuantity.Split(',').Select(int.Parse).ToList();
//                    for (int i = 0; i < selectedStockIDs.Count(); i++)
//                    {
//                        if (selectedStockQuantities[i] > 0)
//                            hasStockItems = true;
//                    }

//                    if (!hasStockItems)
//                        return Json("Please select items from stock");
//                }
//            }

//            if (model.SettingDTO.IsStock)
//            {
//                bool validStock = true;
//                var selectedStockIDs = model.ProductIDs.Split(',').Select(int.Parse).ToList();
//                var selectedStockQuantities = model.ProductsQuantity.Split(',').Select(int.Parse).ToList();
//                for (int i = 0; i < selectedStockIDs.Count(); i++)
//                {
//                    if (selectedStockQuantities[i] > 0)
//                    {
//                        var VendorProductRequest = new VendorProductRequest
//                        {
//                            context = _context,
//                            VendorProductDTO = new VendorProductDTO { VendorProductId = selectedStockIDs[i] },
//                        };
//                        var product = VendorProductService.GetProductById(VendorProductRequest).VendorProductDTO;
//                        if (product.AvailableStock < selectedStockQuantities[i])
//                            validStock = false;
//                    }
//                }

//                if (!validStock)
//                    return Json("Item quantity is less than stock");
//            }

//            if (model.SettingDTO.PackingId.HasValue)
//            {
//                Packing pack = _context.Packing.FirstOrDefault(u => u.Id == model.SettingDTO.PackingId);
//                if (pack.Count <= 0)
//                    return Json("Selected packing is not available");
//            }

//            #endregion

//            var request = new ShipmentRequest();
//            request.context = _context;
//            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//            request.UserID = AuthHelper.GetClaimValue(User, "UserID");

//            if (request.RoleID == (int)EnumRole.Vendor)
//                model.VendorDetailsDTO = new VendorDetailsDTO() { VendorId = request.UserID };

//            request.ShipDTO = model;

//            var response = BL.Services.ShipmentService.AddShipment(request);

//            ViewBag.Vendors = GeneralHelper.GetUsers((int)EnumRole.Vendor, _context);
//            ViewBag.delivery = GeneralHelper.GetUsers((int)EnumRole.DeliveryMan, _context);
//            ViewBag.branch = _context.Branch.ToList();
//            ViewBag.areas = _context.City.ToList();

//            return Json(response.Message);
//        }

//        [AuthorizePerRole("ShipmentAdd")]
//        public IActionResult Add(string ActionType = null)
//        {
//            ViewBag.PackingType = PackingTypeService.GetPackingType(new PackingTypeRequest { context = _context }).PackingTypeDTOs;
//            ViewBag.ShipmentService = _context.ShipmentService.Where(s => s.Id == (int)EnumShipmentService.PickupDelivery);
//            ViewBag.ZoneList = ZoneService.GetZones(new ZoneRequest { context = _context }).ZoneDTOs;
//            ViewBag.BarnchList = BranchService.GetBranchs(new BranchRequest { context = _context }).BranchDTOs.ToList();
//            ViewBag.Vendors = GeneralHelper.GetUsers((int)EnumRole.Vendor, _context);
//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView("/Views/Shared/Shipment/_AddOrder.cshtml");
//            else
//                return View();
//        }

//        [AllowAnonymous]
//        [HttpGet]
//        public IActionResult GetTotalPrice(int AreaID, int PackingID)
//        {
//            double ShippingFees = 0;
//            double PackingFees = 0;
//            string ZoneName = "";

//            if (AreaID > 0)
//            {
//                var Area = _context.City.FirstOrDefault(z => z.Id == AreaID);
//                ZoneTax zonetax = _context.ZoneTax.FirstOrDefault(z => z.ZoneId == Area.ZoneId);
//                var zone = _context.Zone.FirstOrDefault(z => z.Id == zonetax.ZoneId);

//                ShippingFees = zonetax.Tax;
//                ZoneName = zone.NameEn;
//            }

//            if (PackingID > 0)
//            {
//                var Packing = _context.Packing.FirstOrDefault(p => p.Id == PackingID);
//                PackingFees = Packing.Price;
//            }

//            return Json(new { ShippingFees = ShippingFees, ZoneName = ZoneName, PackingFees = PackingFees });
//        }

//        [AllowAnonymous]
//        [HttpGet]
//        public IActionResult GetAreaFees(int AreaID)
//        {
//            double ShippingFees = 0;
//            string ZoneName = "";

//            if (AreaID > 0)
//            {
//                var Area = _context.City.FirstOrDefault(z => z.Id == AreaID);
//                ZoneTax zonetax = _context.ZoneTax.FirstOrDefault(z => z.ZoneId == Area.ZoneId);
//                var zone = _context.Zone.FirstOrDefault(z => z.Id == zonetax.ZoneId);

//                ShippingFees = zonetax.Tax;
//                ZoneName = zone.NameEn;
//            }

//            return Json(new { ShippingFees = ShippingFees, ZoneName = ZoneName });
//        }

//        [AuthorizePerRole("ShipmentRefund")]
//        public IActionResult Refund(string s)
//        {
//            int shipID = int.Parse(EncryptionManager.Decrypt(s));

//            ViewBag.ZoneList = ZoneService.GetZones(new ZoneRequest { context = _context }).ZoneDTOs;

//            //*** Get shipment details
//            var model = new ShipDTO();
//            model.ShipmentId = shipID;
//            var shipRequest = new ShipmentRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ShipDTO = model,

//                HasSettingDTO = true,
//                HasCustomerDetailsDTO = true,
//                HasVendorDetailsDTO = true,
//                HasFeesDetailsDTO = true,
//                HasFollowUpDTOs = true,
//                HasShipItemDTOs = true,
//            };
//            var shipResponse = LegendCR.BL.Services.ShipmentService.GetShipment(shipRequest);

//            //*** Get Account balance
//            var accRequest = new AccountRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                AccountDTO = new CommonDefinitions.DTO.CashDTOs.AccountDTO { UserId = shipResponse.ShipDTO.VendorDetailsDTO.VendorId },
//                HasTransactionsDTOs = false
//            };
//            var accResponse = LegendCR.BL.Services.AccountService.GetAccount(accRequest);
//            shipResponse.ShipDTO.VendorBalance = accResponse.AccountDTO.Balance;

//            //*** Get shipment transactions
//            var trRequest = new AccountTransactionRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                AccountTransactionDTO = new CommonDefinitions.DTO.CashDTOs.AccountTransactionDTO { ShipmentId = shipResponse.ShipDTO.ShipmentId },
//                applyFilter = true
//            };
//            var trResponse = LegendCR.BL.Services.AccountTransactionService.GetAllTransactions(trRequest);
//            var redTransaction = trResponse.AccountTransactionDTOs.FirstOrDefault(r => r.ReceiverId == (int)EnumAccountId.RED_Main_Account);
//            var vendorTransaction = trResponse.AccountTransactionDTOs.FirstOrDefault(r => r.ReceiverId == accResponse.AccountDTO.AccountId);

//            shipResponse.ShipDTO.FeesDetailsDTO.TotalReceived = vendorTransaction.Total + redTransaction.Total;
//            shipResponse.ShipDTO.FeesDetailsDTO.VendorCash = vendorTransaction.Total;
//            shipResponse.ShipDTO.FeesDetailsDTO.TotalRedFees = redTransaction.Total;

//            return View(shipResponse.ShipDTO);
//        }

//        [AuthorizePerRole("ShipmentAdd")]
//        [HttpPost]
//        public IActionResult AddRefund(ShipDTO model)
//        {
//            #region Data Validations

//            // Check total cash
//            if (model.RefundCash <= 0)
//            {
//                TempData["ErrorMsg"] = "Refund cash can't be negative value";
//                return RedirectToAction("Refund", new { s = EncryptionManager.Encrypt(model.ShipmentId.ToString()) });
//            }

//            // Check vendor balance
//            var accRequest = new AccountRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                AccountDTO = new CommonDefinitions.DTO.CashDTOs.AccountDTO { UserId = model.VendorDetailsDTO.VendorId },
//                HasTransactionsDTOs = false
//            };
//            var accResponse = LegendCR.BL.Services.AccountService.GetAccount(accRequest);
//            ZoneTax zoneTax = _context.ZoneTax.FirstOrDefault(z => z.ZoneId == model.ZoneId);

//            if ((model.RefundCash + zoneTax.Tax) > accResponse.AccountDTO.Balance)
//            {
//                TempData["ErrorMsg"] = "You don't have enough balance";
//                return RedirectToAction("Refund", new { s = EncryptionManager.Encrypt(model.ShipmentId.ToString()) });
//            }

//            #endregion

//            var request = new ShipmentRequest();
//            request.context = _context;
//            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//            request.UserID = AuthHelper.GetClaimValue(User, "UserID");

//            if (request.RoleID == (int)EnumRole.Vendor)
//                model.VendorDetailsDTO = new VendorDetailsDTO() { VendorId = request.UserID };

//            request.ShipDTO = model;

//            var response = BL.Services.ShipmentService.AddShipmentRefund(request);

//            if (response.Success)
//                TempData["SuccessMsg"] = response.Message;
//            else
//                TempData["ErrorMsg"] = response.Message;

//            return RedirectToAction("All");
//        }

//        [AuthorizePerRole("ShipmentEdit")]
//        public IActionResult Edit(int shipID)
//        {
//            //var ShipmentID = int.Parse(EncryptionHelper.Decrypt(OrderID));

//            ViewBag.areas = _context.City.ToList();
//            ViewBag.Status = _context.Status.ToList();
//            ViewBag.Packing = _context.PackingType.ToList();
//            ViewBag.ShipmentService = _context.ShipmentService.ToList();
//            ViewBag.PackingType = _context.PackingType.ToList();
//            ViewBag.PackingBox = _context.Packing.Where(p => p.PackingTypeId == 1).ToList();
//            ViewBag.AreasList = _context.City.ToList();
//            ViewBag.vendors = GeneralHelper.GetUsers((int)EnumRole.Vendor, _context);
//            ViewBag.ZoneList = _context.Zone.Where(z => !z.IsDeleted).ToList();

//            var model = new ShipDTO();
//            model.ShipmentId = shipID;
//            var request = new ShipmentRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ShipDTO = model,

//                HasSettingDTO = true,
//                HasCustomerDetailsDTO = true,
//                HasVendorDetailsDTO = true,
//                HasFeesDetailsDTO = true,
//                HasFollowUpDTOs = true,
//                HasShipItemDTOs = true,
//            };

//            var response = LegendCR.BL.Services.ShipmentService.GetShipment(request);

//            return View(response.ShipDTO);
//        }

//        [AuthorizePerRole("ShipmentEdit")]
//        [HttpPost]
//        public ActionResult Edit(ShipDTO model)
//        {
//            var request = new ShipmentRequest();
//            request.context = _context;
//            request.RoleID = AuthHelper.GetClaimValue(User, "RoleID");
//            request.UserID = AuthHelper.GetClaimValue(User, "UserID");
//            request.ShipDTO = model;

//            var response = BL.Services.ShipmentService.EditShipment(request);

//            ViewBag.Vendors = GeneralHelper.GetUsers((int)EnumRole.Vendor, _context);
//            ViewBag.delivery = GeneralHelper.GetUsers((int)EnumRole.DeliveryMan, _context);
//            ViewBag.branch = _context.Branch.ToList();
//            ViewBag.areas = _context.City.ToList();

//            return Json(response.Message);
//        }

//        [AuthorizePerRole("ShipmentEditAddress")]
//        [HttpPost]
//        public IActionResult EditAddress(int shipID, string Address, string Location, string Landmark, int Floor, int Apartment)
//        {
//            var request = new ShipmentRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                ShipDTO = new ShipDTO
//                {
//                    ShipmentId = shipID,
//                    CustomerDetailsDTO = new CustomerDetailsDTO
//                    {
//                        CustomerAddress = Address,
//                        Location = Location,
//                        Landmark = Landmark,
//                        Floor = Floor,
//                        Apartment = Apartment
//                    }
//                }
//            };

//            var response = BL.Services.ShipmentService.EditAddress(request);

//            return Json(new { success = response.Success, message = response.Message });
//        }

//        [AuthorizePerRole("ShipmentCancel")]
//        public IActionResult Cancel(int shipID, string Comment, bool IsTripCompleted, int ReasonType)
//        {
//            var request = new ShipmentRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                ShipDTO = new ShipDTO
//                {
//                    ShipmentId = shipID,
//                    //StatusId = (int)EnumStatus.Cancelled,
//                    CancelComment = Comment,
//                    IsTripCompleted = IsTripCompleted,
//                    ProblemDTOs = new List<ShipProblemDTO>
//                    {
//                        new ShipProblemDTO
//                        {
//                            ProblemTypeId = ReasonType,
//                            Note = Comment,
//                        }
//                    }
//                }
//            };

//            var response = LegendCR.BL.Services.ShipmentService.Cancel(request);

//            return Json(new { success = response.Success, message = response.Message });
//        }

//        [AuthorizePerRole("ShipmentCallHisory")]
//        public IActionResult CallHisory(int shipID, int CallAnswer, int CallNotAnswer)
//        {
//            var request = new ShipmentRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                ShipDTO = new ShipDTO
//                {
//                    ShipmentId = shipID,
//                    CustomerFollowUpDTO = new CustomerFollowUpDTO
//                    {
//                        CallAnswerCount = CallAnswer,
//                        CallNotAnswerCount = CallNotAnswer,
//                    }
//                }
//            };

//            var response = BL.Services.ShipmentService.EditCallHistory(request);

//            return Json(new { success = response.Success, message = response.Message });
//        }

//        [AuthorizePerRole("ShipmentPostpone")]
//        public IActionResult Postpone(int shipID, DateTime? Date, DateTime? From, DateTime? To, string Note, bool IsConfirmed)
//        {
//            var request = new ShipmentRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                ShipDTO = new ShipDTO
//                {
//                    ShipmentId = shipID,
//                    CustomerFollowUpDTO = new CustomerFollowUpDTO
//                    {
//                        NextCallOn = Date.Value,
//                        NextCallTimeFrom = From.Value,
//                        NextCallTimeTo = To.Value,
//                        IsConfirmed = IsConfirmed,
//                        Note = Note
//                    }
//                }
//            };

//            var response = BL.Services.ShipmentService.Postpone(request);

//            return Json(new { success = response.Success, message = response.Message });
//        }

//        [AuthorizePerRole("ShipmentPrint")]
//        public IActionResult Print(int shipID)
//        {
//            var request = new ShipmentRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                HasCustomerDetailsDTO = true,
//                HasVendorDetailsDTO = true,
//                HasSettingDTO = true,
//                HasFeesDetailsDTO = true,
//                HasShipItemDTOs = true,

//                ShipDTO = new ShipDTO
//                {
//                    ShipmentId = shipID
//                }
//            };

//            var response = BL.Services.ShipmentService.GetShipment(request);

//            string footer = "--no-stop-slow-scripts --javascript-delay 10000  --footer-right \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";
//            var pdfFile = new ViewAsPdf("~/Views/Shipment/Print.cshtml", response.ShipDTO)
//            {
//                CustomSwitches = footer,
//                PageMargins = new Margins(2, 2, 2, 2),
//                PageOrientation = Orientation.Portrait,
//                PageSize = Size.A4
//            };
//            return pdfFile;
//        }

//        // *** Export To PDF ***

//        //[HttpPost]
//        //public FileResult Export(string GridHtml)
//        //{
//        //    using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(GridHtml)))
//        //    {
//        //        ByteArrayOutputStream byteArrayOutputStream = new ByteArrayOutputStream();
//        //        PdfWriter writer = new PdfWriter(byteArrayOutputStream);
//        //        PdfDocument pdfDocument = new PdfDocument(writer);
//        //        pdfDocument.SetDefaultPageSize(PageSize.A4);
//        //        HtmlConverter.ConvertToPdf(stream, pdfDocument);
//        //        pdfDocument.Close();
//        //        return File(byteArrayOutputStream.ToArray(), "application/pdf", "Grid.pdf");
//        //    }
//        //}

//        //[HttpGet]
//        //public FileResult GeneratePDF(string html)
//        //{
//        //    html = html.Replace("strTag", "<").Replace("EndTag", ">");
//        //    HtmlToPdf objhtml = new HtmlToPdf();
//        //    PdfDocument objdoc = objhtml.ConvertHtmlString(html);
//        //    byte[] pdf = objdoc.Save();
//        //    objdoc.Close();
//        //    return File(pdf, "application/pdf");
//        //}

//        [AuthorizePerRole("ShipmentPrint")]
//        public IActionResult PrintAll(string ShipmentsIds)
//        {
//            List<int> ShipmentsList = ShipmentsIds.Split(',').Select<string, int>(int.Parse).ToList();
//            var request = new ShipmentRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                applyFilter = true,
//                ShipDTO = new ShipDTO { ShipmentIDs = ShipmentsList },
//                HasCustomerDetailsDTO = true,
//                HasVendorDetailsDTO = true,
//                HasSettingDTO = true,
//                HasFeesDetailsDTO = true,
//                HasShipItemDTOs = true,
//            };

//            var response = BL.Services.ShipmentService.GetAllShipments(request);

//            string footer = "--no-stop-slow-scripts --javascript-delay 10000  --footer-right \"Date: [date] [time]\" " + "--footer-center \"Page: [page] of [toPage]\" --footer-line --footer-font-size \"9\" --footer-spacing 5 --footer-font-name \"calibri light\"";
//            var pdfFile = new ViewAsPdf("~/Views/Shipment/PrintAll.cshtml", response.ShipDTOs)
//            {
//                CustomSwitches = footer,
//                PageMargins = new Margins(2, 2, 2, 2),
//                PageOrientation = Orientation.Portrait,
//                PageSize = Size.A4
//            };
//            return pdfFile;
//        }

//        [AuthorizePerRole("ShipmentReturnToVendor")]
//        public IActionResult ReturnToVendor(int shipID)
//        {
//            var request = new ShipmentRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                ShipDTO = new ShipDTO
//                {
//                    ShipmentId = shipID,
//                    StatusId = (int)EnumStatus.Ready_For_Return,
//                }
//            };

//            var response = LegendCR.BL.Services.ShipmentService.ChangeStatus(request);

//            if (response.Success)
//                TempData["SuccessMsg"] = response.Message;
//            else
//                TempData["ErrorMsg"] = response.Message;

//            return RedirectToAction("All");
//        }

//        #endregion

//        #region Shipment problem actions

//        [AuthorizePerRole("ShipmentAddProblem")]
//        public IActionResult AddProblem(int shipID, string Note, int ProblemTypeID)
//        {
//            var request = new ShipmentRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                ShipDTO = new ShipDTO
//                {
//                    ShipmentId = shipID,
//                    ProblemDTOs = new List<ShipProblemDTO>
//                    {
//                        new ShipProblemDTO
//                        {
//                            ProblemTypeId = ProblemTypeID,
//                            Note = Note,
//                            IsCourierProblem = false
//                        }
//                    }
//                }
//            };

//            var response = BL.Services.ShipmentService.AddProblem(request);

//            return Json(new { success = response.Success, message = response.Message });
//        }

//        [AuthorizePerRole("ShipmentReportProblemToVendor")]
//        public IActionResult ReportToVendor(int shipID, string Note, int ProblemTypeID, bool IsCoruierProblem)
//        {
//            var request = new ShipmentRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                ShipDTO = new ShipDTO
//                {
//                    ShipmentId = shipID,
//                    ProblemDTOs = new List<ShipProblemDTO>
//                    {
//                        new ShipProblemDTO
//                        {
//                            ProblemTypeId = ProblemTypeID,
//                            Note = Note,
//                            IsReportToVendor = true,
//                            IsCourierProblem = IsCoruierProblem
//                        }
//                    }
//                }
//            };

//            var response = BL.Services.ShipmentService.ReportProblemToVendor(request);

//            return Json(new { success = response.Success, message = response.Message });
//        }

//        [AuthorizePerRole("ShipmentSolveProblem")]
//        public IActionResult SolveProblem(int shipID, string Solution)
//        {
//            var request = new ShipmentRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                ShipDTO = new ShipDTO
//                {
//                    ShipmentId = shipID,
//                    ProblemDTOs = new List<ShipProblemDTO>
//                    {
//                        new ShipProblemDTO
//                        {
//                            IsSolved = true,
//                            Solution = Solution,
//                            IsCourierProblem = false
//                        }
//                    }
//                }
//            };

//            var response = BL.Services.ShipmentService.SolveProblem(request);

//            return Json(response.Message);
//        }

//        [AuthorizePerRole("ShipmentSolveProblem")]
//        public IActionResult SolveCourierProblem(int shipID, string Solution)
//        {
//            var request = new ShipmentRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                ShipDTO = new ShipDTO
//                {
//                    ShipmentId = shipID,
//                    ProblemDTOs = new List<ShipProblemDTO>
//                    {
//                        new ShipProblemDTO
//                        {
//                            IsSolved = true,
//                            Solution = Solution,
//                            IsCourierProblem = true
//                        }
//                    }
//                }
//            };

//            var response = BL.Services.ShipmentService.SolveProblem(request);

//            return Json(response.Message);
//        }

//        [AuthorizePerRole("ShipmentReplyToCourier")]
//        public IActionResult ReplyToCourier(int shipID, string Solution)
//        {
//            var request = new ShipmentRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                ShipDTO = new ShipDTO
//                {
//                    ShipmentId = shipID,
//                    ProblemDTOs = new List<ShipProblemDTO>
//                    {
//                        new ShipProblemDTO
//                        {
//                            IsSolved = false,
//                            Solution = Solution,
//                            IsCourierProblem = true
//                        }
//                    }
//                }
//            };

//            var response = BL.Services.ShipmentService.SolveProblem(request);

//            return Json(response.Message);
//        }

//        [AuthorizePerRole("ShipmentDeleteProblem")]
//        public IActionResult DeleteProblem(int shipID)
//        {
//            var request = new ShipmentRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                ShipDTO = new ShipDTO
//                {
//                    ShipmentId = shipID,
//                    ProblemDTOs = new List<ShipProblemDTO>
//                    {
//                        new ShipProblemDTO
//                        {
//                            IsDeleted = true,
//                            IsCourierProblem = false
//                        }
//                    }
//                }
//            };

//            var response = BL.Services.ShipmentService.DeleteProblem(request);

//            return Json(response.Message);
//        }

//        [AuthorizePerRole("ShipmentDeleteProblem")]
//        public IActionResult DeleteCourierProblem(int shipID)
//        {
//            var request = new ShipmentRequest
//            {
//                context = _context,
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                ShipDTO = new ShipDTO
//                {
//                    ShipmentId = shipID,
//                    ProblemDTOs = new List<ShipProblemDTO>
//                    {
//                        new ShipProblemDTO
//                        {
//                            IsDeleted = true,
//                            IsCourierProblem = true
//                        }
//                    }
//                }
//            };

//            var response = BL.Services.ShipmentService.DeleteProblem(request);

//            return Json(response.Message);
//        }

//        #endregion

//        #region Shipment list

//        [AuthorizePerRole("ShipmentsList")]
//        public IActionResult All(string Search, string OrderByColumn, bool? IsDesc, int PageIndex, DateTime? From, DateTime? To,
//            int ZoneId, int StatusId, int VendorId, int AreaId, int Filter, string ActionType = null)
//        {
//            ViewModel<ShipDTO> ViewData = new ViewModel<ShipDTO>();

//            if (ActionType != Constants.ActionType.Table)
//            {
//                ViewData.Lookup = BaseHelper.GetLookup(new List<byte> {
//                    (byte)EnumSelectListType.Zone,
//                    (byte)EnumSelectListType.Vendor,
//                    (byte)EnumSelectListType.Area,
//                    (byte)EnumSelectListType.Status
//                }, _context);
//            }
//            ViewBag.ProblemType = _context.ProblemType.Where(r => r.Type == (int)EnumProblemType.ProblemType).ToList();
//            ViewBag.CancelReason = _context.ProblemType.Where(r => r.Type == (int)EnumProblemType.CancelType).ToList();

//            var filter = new ShipDTO();

//            if (StatusId == (int)EnumStatus.Current)
//            {
//                filter.StatusIDs = new List<int>
//                {
//                    (int)EnumStatus.Ready_For_Delivery,
//                    (int)EnumStatus.Assigned_For_Delivery,
//                    (int)EnumStatus.Out_For_Delivery,

//                    (int)EnumStatus.Ready_For_Pickup,
//                    (int)EnumStatus.Assigned_For_Pickup,
//                    (int)EnumStatus.Picked_Up,

//                    (int)EnumStatus.At_Warehouse,
//                    (int)EnumStatus.Postponed,

//                    (int)EnumStatus.Ready_For_Return,
//                    (int)EnumStatus.Assigned_For_Return,
//                    (int)EnumStatus.Out_For_Return,

//                    (int)EnumStatus.Ready_For_Refund,
//                    (int)EnumStatus.Assigned_For_Refund,
//                    (int)EnumStatus.Out_For_Refund,
//                };
//            }
//            else if (StatusId == (int)EnumStatus.CourierReturn)
//            {
//                //filter.StatusIDs = new List<int> { (int)EnumStatus.At_Warehouse, (int)EnumStatus.Ready_For_Return };
//                filter.IsCourierReturned = true;
//                //filter.ReturnCount = 1;
//            }
//            else if (StatusId == (int)EnumStatus.Cancelled)
//                filter.StatusId = (int)EnumStatus.Cancelled;
//            else if (StatusId == (int)EnumStatus.Done)
//                filter.StatusIDs = new List<int> { (int)EnumStatus.Delivered, (int)EnumStatus.Paid_To_Vendor, (int)EnumStatus.Refunded };
//            else
//                filter.StatusId = StatusId;

//            filter.ZoneId = ZoneId;
//            filter.Search = Search;
//            filter.VendorDetailsDTO = new VendorDetailsDTO { VendorId = VendorId };
//            filter.AreaId = AreaId;
//            if (From.HasValue)
//                filter.DateFrom = From.Value;
//            if (To.HasValue)
//                filter.DateTo = To.Value;

//            var request = new ShipmentRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ShipDTO = filter,
//                IsDesc = IsDesc ?? true,
//                PageIndex = PageIndex,
//                PageSize = BaseHelper.Constants.PageSize,
//                OrderByColumn = OrderByColumn,
//                applyFilter = true,

//                HasCustomerDetailsDTO = true,
//                HasVendorDetailsDTO = true,
//                HasCustomerFollowUpDTO = true,
//                HasFeesDetailsDTO = true,
//                HasProblemDTOs = true,
//                HasSettingDTO = true,
//                HasFollowUpDTOs = true,
//            };

//            var response = BL.Services.ShipmentService.GetAllShipments(request);

//            ViewData.ObjDTOs = response.ShipDTOs;

//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView(ViewData);
//            else if (ActionType == Constants.ActionType.Table)
//            {
//                if (StatusId == (int)EnumStatus.All)
//                    return PartialView("_All", response.ShipDTOs);

//                else if (StatusId == (int)EnumStatus.Current)
//                    return PartialView("_Current", response.ShipDTOs);

//                else if (StatusId == (int)EnumStatus.CourierReturn)
//                    return PartialView("_Returned", response.ShipDTOs);

//                else if (StatusId == (int)EnumStatus.Cancelled)
//                    return PartialView("_Cancelled", response.ShipDTOs);

//                else if (StatusId == (int)EnumStatus.Done)
//                    return PartialView("_Done", response.ShipDTOs);

//                else return PartialView("_All", response.ShipDTOs);
//            }

//            else
//                return View(ViewData);
//        }

//        #endregion

//        #region Assign shipments

//        [AuthorizePerRole("ShipmentsAssignList")]
//        public IActionResult Assign(int AreaId, string Search, string OrderByColumn, bool? IsDesc, int PageIndex, int ZoneID = 0, string ActionType = null)
//        {
//            ViewModel<ShipDTO> ViewData = new ViewModel<ShipDTO>();

//            ViewData.Lookup = BaseHelper.GetLookup(
//                new List<byte>
//                {
//                    (byte)EnumSelectListType.Courier,
//                    (byte)EnumSelectListType.Area,
//                }, _context);

//            var filter = new ShipDTO();
//            filter.StatusIDs = new List<int> { (int)EnumStatus.Ready_For_Delivery, (int)EnumStatus.Ready_For_Return, (int)EnumStatus.Ready_For_Refund };
//            filter.ZoneId = ZoneID;
//            filter.AreaId = AreaId;
//            filter.Search = Search;

//            var request = new ShipmentRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ShipDTO = filter,
//                IsDesc = IsDesc ?? true,
//                PageIndex = PageIndex,
//                OrderByColumn = OrderByColumn,
//                applyFilter = true,

//                HasCustomerDetailsDTO = true,
//                HasVendorDetailsDTO = true,
//                HasFeesDetailsDTO = true,
//                HasSettingDTO = true,
//            };

//            var response = BL.Services.ShipmentService.GetAllShipments(request);
//            ViewData.ObjDTOs = response.ShipDTOs;
//            if (ActionType == Constants.ActionType.PartialView)
//                return PartialView(ViewData);
//            else if (ActionType == Constants.ActionType.Table)
//                return PartialView("_Assign", ViewData);
//            else
//                return View(ViewData);
//        }

//        [AuthorizePerRole("ShipmentsAssign")]
//        [HttpPost]
//        public IActionResult Assign(List<int> ShipmentIDs, int ddldriver)
//        {
//            var request = new ShipmentRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ShipDTO = new ShipDTO { ShipmentIDs = ShipmentIDs, DeliveryManId = ddldriver }
//            };

//            var response = BL.Services.ShipmentService.Assign(request);

//            if (response.Success)
//                TempData["SuccessMsg"] = response.Message;
//            else
//                TempData["ErrorMsg"] = response.Message;


//            return RedirectToAction("Assign");
//        }

//        [AuthorizePerRole("ShipmentsUnAssign")]
//        [HttpPost]
//        public IActionResult Unassigned(int ShipmentID)
//        {
//            var request = new ShipmentRequest
//            {
//                RoleID = AuthHelper.GetClaimValue(User, "RoleID"),
//                UserID = AuthHelper.GetClaimValue(User, "UserID"),
//                context = _context,
//                ShipDTO = new ShipDTO { ShipmentId = ShipmentID }
//            };

//            var response = BL.Services.ShipmentService.Unassigned(request);

//            //if (response.Success)
//            //    TempData["SuccessMsg"] = response.Message;
//            //else
//            //    TempData["ErrorMsg"] = response.Message;

//            //return RedirectToAction("All");

//            return Json(response);
//        }

//        #endregion
//    }
//}