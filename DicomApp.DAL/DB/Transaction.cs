using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public string RefId { get; set; }
        public string PaymentId { get; set; }
        public byte TypeId { get; set; }
        public int? AdvertisementId { get; set; }
        public int BuyerId { get; set; }
        public int Amount { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Advertisement Advertisement { get; set; }
        public virtual CommonUser Buyer { get; set; }
        public virtual CommonUser CreatedByNavigation { get; set; }
        public virtual CommonUser LastModifiedByNavigation { get; set; }
    }
}
