using System;
using System.Collections.Generic;
using DicomApp.DAL.DB;

namespace DicomApp.CommonDefinitions.DTO.CashDTOs
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }
        public string RefId { get; set; }
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
        public string Search { get; set; }
        public string PaymentId { get; set; }
        public int? ServiceId { get; set; }
        public TransactionTypeEnum TransactionTypeEnum { get; set; }
    }
}
