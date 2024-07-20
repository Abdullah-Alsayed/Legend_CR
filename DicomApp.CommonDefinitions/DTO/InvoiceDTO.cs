using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.DAL.DB;

namespace DicomApp.CommonDefinitions.DTO
{
    public class InvoiceDTO
    {
        public int InvoiceId { get; set; }
        public string RefId { get; set; }
        public int? AdvertisementId { get; set; }
        public int? GamerServiceId { get; set; }
        public int Price { get; set; }
        public bool IsRefund { get; set; }
        public int InvoiceType { get; set; }

        public string Search { get; set; }
        public int LessPrice { get; set; }
        public int GreeterPrice { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public AdsDTO Advertisement { get; set; }
        public ServiceDTO GamerService { get; set; }
        public int ItemId { get; set; }
    }
}
