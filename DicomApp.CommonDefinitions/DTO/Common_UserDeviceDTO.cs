using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using DicomApp.DAL.DB;
using System.Linq;
using DicomApp.Helpers;

namespace DicomApp.CommonDefinitions.DTO
{
    public class Common_UserDeviceDTO
    {
        public int ID { get; set; }
        public int Common_UserID { get; set; }
        public string DeviceName { get; set; }
        public string DeviceIMEI { get; set; }
        public string DeviceType { get; set; }
        public string DeviceOSVersion { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceEmail { get; set; }
        public bool EnableNotification { get; set; }
        public string AuthToken { get; set; }
        public string AuthIP { get; set; }

        [IgnoreDataMember]
        public DateTime AuthCreationDate { get; set; }
        public string AuthCreationDateStr
        {
            get { return DateTimeHelper.ToDate(AuthCreationDate); }
            set { }
        }

        [IgnoreDataMember]
        public DateTime AuthExpirationDate { get; set; }
        public string AuthExpirationDateStr
        {
            get { return DateTimeHelper.ToDate(AuthExpirationDate); }
            set { }
        }
        public bool IsLoggedIn { get; set; }
        public string DeviceMobileNumber { get; set; }

        [IgnoreDataMember]
        public DateTime? LastActiveDate { get; set; }
        public string LastActiveDateStr
        {
            get { return DateTimeHelper.ToDate(LastActiveDate ?? DateTime.Now); }
            set { }
        }
        public string Lang { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        [IgnoreDataMember]
        public DateTime? LastUpdateDate { get; set; }
        public string LastUpdateDateStr
        {
            get { return DateTimeHelper.ToDate(LastUpdateDate ?? DateTime.Now); }
            set { }
        }

        [IgnoreDataMember]
        public DateTime CreationDate { get; set; }
        public string CreationDateStr
        {
            get { return DateTimeHelper.ToDate(CreationDate); }
            set { }
        }
        public bool IsDeleted { get; set; }
        public bool? IsGoogleSupport { get; set; }
    }
}