using System;
using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.DTO.CashDTOs
{
    public class AccountDTO
    {
        public int AccountId { get; set; }

        public string Name { get; set; }
        public string RefId { get; set; }
        public double Balance { get; set; }
        public int UserId { get; set; }
        public string BAN { get; set; }
        public string IBAN { get; set; }
        public string BankName { get; set; }
        public int? VodafoneCash { get; set; }
        public string InstaPay { get; set; }
        public int? RoleId { get; set; }
        public string VendorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<TransactionDTO> TransactionDTOs { get; set; }
        public string Search { get; set; }
    }
}
