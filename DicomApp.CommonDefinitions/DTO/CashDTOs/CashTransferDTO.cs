using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;

namespace DicomApp.CommonDefinitions.DTO.CashDTOs
{
    public class CashTransferDTO
    {
        public int CashTransferId { get; set; }
        public string RefId { get; set; }
        public byte TypeId { get; set; }
        public int ReceiverId { get; set; }
        public int VendorId { get; set; }
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
        public string OTP { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string TransIDs { get; set; }

        public IEnumerable<AccountTransactionDTO> AccountTransactionDTOs { get; set; }
        public List<int> PaidShipsIDs { get; set; }
        public string Search { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
