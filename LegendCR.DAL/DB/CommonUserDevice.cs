
using System;
using System.Collections.Generic;

namespace LegendCR.DAL.DB
{
    public partial class CommonUserDevice
    {
        public int Id { get; set; }
        public int CommonUserId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceImei { get; set; }
        public string DeviceType { get; set; }
        public string DeviceOsversion { get; set; }
        public string DeviceToken { get; set; }
        public string DeviceEmail { get; set; }
        public bool? EnableNotification { get; set; }
        public string AuthToken { get; set; }
        public string AuthIp { get; set; }
        public DateTime AuthCreationDate { get; set; }
        public DateTime AuthExpirationDate { get; set; }
        public bool IsLoggedIn { get; set; }
        public string DeviceMobileNumber { get; set; }
        public DateTime? LastActiveDate { get; set; }
        public string Lang { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsGoogleSupport { get; set; }

        public virtual CommonUser CommonUser { get; set; }
    }
}
