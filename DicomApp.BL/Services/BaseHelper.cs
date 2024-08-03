using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomDB.BL.Services;
using DicomDB.CommonDefinitions.Requests;
using Microsoft.AspNetCore.Http;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;

namespace DicomApp.BL.Services
{
    public class BaseHelper
    {
        public static class Constants
        {
            public const int PageSize = 50;
            public const int MaxFreeWeight = 3;
            public const int MinFreeShipCount = 5;
            public const int PartialDeliveryFees = 10;
            public const int PickupFees = 20;
            public const int TripFees = 20;
            public const int CancelFees = 10;
            public const int RefundFees = 0;
        }

        public static int PercentageCalculation(double PresentMonth, double LastMonth)
        {
            if (PresentMonth > LastMonth) // Present Month is GREATER
            {
                var increaseValue = PresentMonth - LastMonth;
                var PercentageIncrease = (increaseValue / PresentMonth) * 100;
                return Convert.ToInt32(PercentageIncrease);
            }
            else if (PresentMonth == LastMonth) // Present Month is EQUAL
            {
                return 0;
            }
            else // Present Month is SMALLER
            {
                var DecreaseValue = LastMonth - PresentMonth;
                var PercentageDdecrease = (DecreaseValue / LastMonth) * 100;
                return Convert.ToInt32(PercentageDdecrease);
            }
        }

        public static double CalcWeightFees(int Weight)
        {
            if (Weight > Constants.MaxFreeWeight)
                return (Weight - Constants.MaxFreeWeight) * 5;
            else
                return 0;
        }

        public static double CalcPickupFees(int shipmentCount)
        {
            if (shipmentCount < Constants.MinFreeShipCount)
                return Constants.PickupFees;
            else
                return 0;
        }

        /// <summary>
        /// Return Pdf Report
        /// </summary>
        /// <typeparam name="T"> Type Of DTOs </typeparam>
        /// <param name="ViewName"> Of Page Name</param>
        /// <param name="EntiteDTO">Type Of DTOs To Returned </param>
        /// <returns></returns>
        public static ViewAsPdf GeneratePDF<T>(string ViewName, T EntiteDTO)
        {
            var pdfFile = new ViewAsPdf(ViewName, EntiteDTO)
            {
                PageMargins = new Margins(2, 2, 2, 2),
                PageOrientation = Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
            return pdfFile;
        }

        /// <summary>
        /// Get List Of Entity For DropDown list
        /// </summary>
        /// <param name="LookType">Select Entitys Need</param>
        /// <param name="Context"></param>
        /// <returns></returns>
        public static LookupDTO GetLookup(List<byte> LookType, ShippingDBContext Context)
        {
            var lookup = new LookupDTO();

            if (LookType.Contains((byte)EnumSelectListType.Zone))
                lookup.ZoneDTOs = ZoneService
                    .GetZones(new ZoneRequest { context = Context })
                    .ZoneDTOs;

            if (LookType.Contains((byte)EnumSelectListType.Courier))
                lookup.CourierDTOs = UserService
                    .GetAllUsers(
                        new UserRequest
                        {
                            context = Context,
                            applyFilter = true,
                            UserDTO = new UserDTO { RoleID = (int)EnumRole.DeliveryMan }
                        }
                    )
                    .UserDTOs;

            if (LookType.Contains((byte)EnumSelectListType.Vendor))
                lookup.VendorDTOs = UserService
                    .GetAllUsers(
                        new UserRequest
                        {
                            context = Context,
                            applyFilter = true,
                            UserDTO = new UserDTO { RoleID = (int)EnumRole.Gamer }
                        }
                    )
                    .UserDTOs;

            if (LookType.Contains((byte)EnumSelectListType.Employee))
                lookup.EmployeeDTOs = UserService
                    .GetAllUsers(
                        new UserRequest
                        {
                            context = Context,
                            applyFilter = true,
                            UserDTO = new UserDTO { StaffOnly = true }
                        }
                    )
                    .UserDTOs;

            if (LookType.Contains((byte)EnumSelectListType.Gamer))
                lookup.UserDTOs = UserService
                    .GetAllUsers(
                        new UserRequest
                        {
                            context = Context,
                            applyFilter = true,
                            UserDTO = new UserDTO { RoleName = SystemConstants.Role.Gamer }
                        }
                    )
                    .UserDTOs;
            if (LookType.Contains((byte)EnumSelectListType.Game))
                lookup.GameDTOs = GameService
                    .GetGames(new GameRequest { context = Context })
                    .GameDTOs;

            if (LookType.Contains((byte)EnumSelectListType.Status))
                lookup.StatusDTOs = StatusService
                    .GetStatus(new StatusRequest { context = Context })
                    .StatusDTOs;

            if (LookType.Contains((byte)EnumSelectListType.Category))
                lookup.CategoryDTOs = CategoryService
                    .GetCategorys(new CategoryRequest { context = Context })
                    .CategoryDTOs;

            if (LookType.Contains((byte)EnumSelectListType.Role))
                lookup.RoleDTOs = RoleService
                    .ListRole(new RoleRequest { context = Context })
                    .RoleDTOs;

            if (LookType.Contains((byte)EnumSelectListType.Area))
                lookup.AreaDTOs = CityService
                    .GetCity(new CityRequest { context = Context })
                    .CityDTOs;
            if (LookType.Contains((byte)EnumSelectListType.Countries))
                lookup.CountryDTOs = CountryService
                    .GetCountry(new CountryRequest { context = Context })
                    .CountryDTOs;
            return lookup;
        }

        public static string UploadImg(IFormFile file, string WebRootPath, string PhotoName = null)
        {
            string Photo = string.Empty;
            string path = string.Empty;
            string fullPath = string.Empty;
            if (file != null)
            {
                var extansion = Path.GetExtension(file.FileName);
                var GuId = Guid.NewGuid().ToString();
                Photo = GuId + extansion;
                path = Path.Combine(WebRootPath, "dist", "images");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                fullPath = Path.Combine(path, Photo);
                file.CopyTo(new FileStream(fullPath, FileMode.Create));
            }
            //Update Photo
            if (PhotoName != null && file != null)
            {
                fullPath = Path.Combine(WebRootPath, "dist", "images", PhotoName);
                System.IO.File.Delete(fullPath);
            }
            if (PhotoName != null && file == null)
                return PhotoName;

            return string.IsNullOrEmpty(Photo) ? SystemConstants.Imges.Default : Photo;
        }

        public static string GenerateRefId(
            EnumRefIdType RefIdType,
            int ID,
            byte TransactionType = (int)EnumTransactionType.Withdraw
        )
        {
            switch (RefIdType)
            {
                case EnumRefIdType.Advertisement:
                    return "AD" + ID;
                case EnumRefIdType.Delivery_Pickup:
                    return "DP" + ID;
                case EnumRefIdType.Stock_Pickup:
                    return "SP" + ID;
                case EnumRefIdType.Cash_Collect:
                    return "CC" + ID;
                case EnumRefIdType.Cash_Delivery:
                    return "CD" + ID;
                case EnumRefIdType.Shipment_Return:
                    return "SR" + ID;
                case EnumRefIdType.Shipment_Refund:
                    return "RF" + ID;
                case EnumRefIdType.GamerService:
                    return "GS" + ID;
                case EnumRefIdType.Invoice:
                    return "IN" + ID;
                case EnumRefIdType.Transaction:
                    return "TR"
                        + ID
                        + (TransactionType == (int)EnumTransactionType.Withdraw ? "W" : "D");
                case EnumRefIdType.Cash_Transfer:
                    return "INV" + ID;
                default:
                    return null;
            }
        }

        public static int GenerateOTP()
        {
            var rndm = new Random();

            var result = rndm.Next(1000, 9999);

            return result;
        }
    }
}
