using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomDB.BL.Services;
using DicomDB.CommonDefinitions.Requests;
using Microsoft.AspNetCore.Http;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;
using System.Collections.Generic;
using System.IO;

namespace DicomApp.BL.Services
{
    public class BaseHelper
    {
        public static class Constants
        {
            public const int PageSize = 25;
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
                lookup.ZoneDTOs = ZoneService.GetZones(new ZoneRequest { context = Context }).ZoneDTOs;

            if (LookType.Contains((byte)EnumSelectListType.Courier))
                lookup.CourierDTOs = UserService.GetAllUsers(new UserRequest { context = Context, applyFilter = true, UserDTO = new UserDTO { RoleID = (int)EnumRole.DeliveryMan } }).UserDTOs;

            if (LookType.Contains((byte)EnumSelectListType.Vendor))
                lookup.VendorDTOs = UserService.GetAllUsers(new UserRequest { context = Context, applyFilter = true, UserDTO = new UserDTO { RoleID = (int)EnumRole.Vendor } }).UserDTOs;

            if (LookType.Contains((byte)EnumSelectListType.Employee))
                lookup.EmployeeDTOs = UserService.GetAllUsers(new UserRequest { context = Context, applyFilter = true, UserDTO = new UserDTO { RoleID = (int)EnumRole.Employee } }).UserDTOs;

            if (LookType.Contains((byte)EnumSelectListType.Status))
                lookup.StatusDTOs = StatusService.GetStatus(new StatusRequest { context = Context }).StatusDTOs;

            if (LookType.Contains((byte)EnumSelectListType.PackingType))
                lookup.PackingTypeDTOs = PackingTypeService.GetPackingType(new PackingTypeRequest { context = Context }).PackingTypeDTOs;

            if (LookType.Contains((byte)EnumSelectListType.Role))
                lookup.RoleDTOs = RoleService.ListRole(new RoleRequest { context = Context }).RoleDTOs;

            if (LookType.Contains((byte)EnumSelectListType.Area))
                lookup.AreaDTOs = CityService.GetCity(new CityRequest { context = Context }).CityDTOs;

            return lookup;
        }

        public static string UploadImg(IFormFile File, string WebRootPath, string ImgUrl)
        {
            string Img = String.Empty;
            if (File != null)
            {
                var extension = Path.GetExtension(File.FileName);
                var GuId = Guid.NewGuid().ToString();
                Img = GuId + extension;
                var UploadFile = Path.Combine(WebRootPath, "dist", "images", Img);
                File.CopyTo(new FileStream(UploadFile, FileMode.Create));
            }
            if (ImgUrl != null && File != null)
            {
                var UploadFile = Path.Combine(WebRootPath, "dist", "images", ImgUrl);
                System.IO.File.Delete(UploadFile);
            }
            if (ImgUrl != null && File == null)
            {
                Img = ImgUrl;
            }
            return Img;
        }

        public static string GenerateRefId(EnumRefIdType RefIdType, int ID, byte TransactionType = (int)EnumTransactionType.Withdraw)
        {
            switch (RefIdType)
            {
                case EnumRefIdType.Shipment:
                    return "SH" + ID;
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
                case EnumRefIdType.Account_Transaction:
                    return "TR" + ID + (TransactionType == (int)EnumTransactionType.Withdraw ? "W" : "D");
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