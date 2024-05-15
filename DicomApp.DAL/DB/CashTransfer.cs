
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class CashTransfer
    {
        public CashTransfer()
        {
            AccountTransaction = new HashSet<AccountTransaction>();
            Shipment = new HashSet<Shipment>();
        }

        public int CashTransferId { get; set; }
        public string RefId { get; set; }
        public byte TypeId { get; set; }
        public int ReceiverId { get; set; }
        public string Name { get; set; }
        public int? ZoneId { get; set; }
        public int? AreaId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Landmark { get; set; }
        public int? Floor { get; set; }
        public int? Apartment { get; set; }
        public bool IsReceived { get; set; }
        public string Otp { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public int LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public virtual City Area { get; set; }
        public virtual CommonUser CreatedByNavigation { get; set; }
        public virtual CommonUser LastModifiedByNavigation { get; set; }
        public virtual Account Receiver { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual ICollection<AccountTransaction> AccountTransaction { get; set; }
        public virtual ICollection<Shipment> Shipment { get; set; }
    }
}
