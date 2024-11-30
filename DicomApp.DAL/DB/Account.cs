using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class Account
    {
        public Account()
        {
            AccountTransactionReceiver = new HashSet<Transaction>();
            AccountTransactionSender = new HashSet<Transaction>();
            AccountTransactionVendor = new HashSet<Transaction>();
        }

        public int AccountId { get; set; }
        public string Name { get; set; }
        public string RefId { get; set; }
        public double Balance { get; set; }
        public string Ban { get; set; }
        public string Iban { get; set; }
        public string BankName { get; set; }
        public int? VodafoneCash { get; set; }
        public string InstaPay { get; set; }
        public int UserId { get; set; }
        public int? RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }

        public virtual CommonUser CreatedByNavigation { get; set; }
        public virtual CommonUser LastModifiedByNavigation { get; set; }
        public virtual Role Role { get; set; }
        public virtual CommonUser User { get; set; }
        public virtual ICollection<Transaction> AccountTransactionReceiver { get; set; }
        public virtual ICollection<Transaction> AccountTransactionSender { get; set; }
        public virtual ICollection<Transaction> AccountTransactionVendor { get; set; }
    }
}
