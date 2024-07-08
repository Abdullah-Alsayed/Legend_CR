using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.DAL.DB;
using Microsoft.AspNetCore.Http;

namespace DicomApp.CommonDefinitions.DTO
{
    public class UserDTO
    {
        public UserDTO()
        {
            //Shipments = new List<ShipmentDTO>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = " Please Enter a Email")]
        [EmailAddress(ErrorMessage = "Please Enter a valid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter New password.")]
        public string NewPassword { get; set; }

        [Compare("Password", ErrorMessage = "Does not match")]
        [Required(ErrorMessage = "Please enter Confirm Password.")]
        public string ConfirmPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Does not match")]
        [Required(ErrorMessage = "Please enter Confirm Password.")]
        public string ConfirmNewPassword { get; set; }

        [Required(ErrorMessage = "Required")]
        public int RoleID { get; set; }

        public string RoleName { get; set; }

        public DateTime? LastLoginDate { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"\d{10,15}$", ErrorMessage = "Not a valid phone number 10-15 digits")]
        public string PhoneNumber { get; set; }

        public string TelegramUserName { get; set; }
        public int? CountryId { get; set; }
        public int Age { get; set; }
        public GenderEnum Gender { get; set; }
        public bool IsLoggedIn { get; set; }

        [Required(ErrorMessage = "Required")]
        public string NationalId { get; set; }

        public bool? IsStaff { get; set; }
        public int? SectionId { get; set; }
        public int? LeadershipId { get; set; }
        public int? AdminstrationId { get; set; }
        public string Code { get; set; }
        public string SectionName { get; set; }
        public DateTime ModificationDate { get; set; }

        [Required(ErrorMessage = "Required")]
        public List<long> SectionsIds { get; set; }

        public string Address { get; set; }
        public int? AreaId { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public int OrderCount { get; set; }
        public string Token { get; set; }
        public bool StaffOnly { get; set; }
        public string FullName { get; set; }
        public string AddressDetails { get; set; }
        public string ProductType { get; set; }
        public int? Averageorders { get; set; }
        public string PageName { get; set; }
        public bool? VisaPayment { get; set; }
        public string Search { get; set; }
        public string LocationUrl { get; set; }
        public string Landmark { get; set; }
        public int? Floor { get; set; }
        public int? Apartment { get; set; }
        public DateTime CreationDate { get; set; }

        public bool HasCourierShipments { get; set; }
        public string Bank { get; set; }
        public string AccountName { get; set; }
        public int? AccountNumber { get; set; }
        public int? IBANNumber { get; set; }
        public int? VodafoneCashNumber { get; set; }
        public string InstaPayAccountName { get; set; }
        public string ImgUrl { get; set; }
        public int? ZoneId { get; set; }
        public string ZoneName { get; set; }
        public string AreaName { get; set; }
        public IFormFile File { get; set; }
        public bool IsDeleted { get; set; }

        public string HashedPassword { get; set; }

        public IEnumerable<AdsDTO> Advertisement { get; set; }

        //Filter
        public bool IsEmployee { get; set; }
    }
}
