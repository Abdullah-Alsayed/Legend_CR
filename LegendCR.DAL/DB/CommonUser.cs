
using System;
using System.Collections.Generic;

namespace LegendCR.DAL.DB
{
    public partial class CommonUser
    {
        public CommonUser()
        {

            CommonUserDevice = new HashSet<CommonUserDevice>();


            ContactUs = new HashSet<ContactUs>();
            FollowUp = new HashSet<FollowUp>();
            Notification = new HashSet<Notification>();

            //ShipmentCustomer = new HashSet<Shipment>();

            //ShipmentDeliveryMan = new HashSet<Shipment>();
            //ShipmentLastModifiedByNavigation = new HashSet<Shipment>();

            //ShipmentVendor = new HashSet<Shipment>();
            UserLocation = new HashSet<UserLocation>();
            VendorProduct = new HashSet<VendorProduct>();
            ZoneCreatedByNavigation = new HashSet<Zone>();
            ZoneLastModifiedByNavigation = new HashSet<Zone>();
        }

        public int Id { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime ModificationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsLoggedIn { get; set; }
        public string NationalId { get; set; }
        public int? AreaId { get; set; }
        public string Code { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Address { get; set; }
        public DateTime? LastLocationDate { get; set; }
        public long? VerificationCode { get; set; }
        public bool? IsVerified { get; set; }
        public string FullName { get; set; }
        public string AddressDetails { get; set; }
        public string ProductType { get; set; }
        public int? Averageorders { get; set; }
        public bool? OpenPackage { get; set; }
        public bool? PartialDelivery { get; set; }
        public bool? VisaPayment { get; set; }
        public string PageName { get; set; }
        public string LocationUrl { get; set; }
        public string Landmark { get; set; }
        public int? Floor { get; set; }
        public int? Apartment { get; set; }
        public int? BranchId { get; set; }
        public string Bank { get; set; }
        public string AccountName { get; set; }
        public int? AccountNumber { get; set; }
        public int? IbanNumber { get; set; }
        public int? VodafoneCashNumber { get; set; }
        public string InstaPayAccountName { get; set; }
        public int? ZoneId { get; set; }
        public string ImgUrl { get; set; }
        public string HashedPassword { get; set; }

        public virtual Role Role { get; set; }

        public virtual ICollection<CommonUserDevice> CommonUserDevice { get; set; }
        public virtual ICollection<ContactUs> ContactUs { get; set; }
        public virtual ICollection<FollowUp> FollowUp { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
        //public virtual ICollection<Shipment> ShipmentCustomer { get; set; }
        //public virtual ICollection<Shipment> ShipmentDeliveryMan { get; set; }
        //public virtual ICollection<Shipment> ShipmentLastModifiedByNavigation { get; set; }
        //public virtual ICollection<Shipment> ShipmentVendor { get; set; }
        public virtual ICollection<UserLocation> UserLocation { get; set; }
        public virtual ICollection<VendorProduct> VendorProduct { get; set; }
        public virtual ICollection<Zone> ZoneCreatedByNavigation { get; set; }
        public virtual ICollection<Zone> ZoneLastModifiedByNavigation { get; set; }
    }
}
