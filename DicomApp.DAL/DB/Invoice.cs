using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DicomApp.DAL.DB
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string RefId { get; set; }
        public int? AdvertisementId { get; set; }
        public int? GamerServiceId { get; set; }
        public int Price { get; set; }
        public bool IsRefund { get; set; }
        public int InvoiceType { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Advertisement Advertisement { get; set; }
        public virtual GamerService GamerService { get; set; }
    }
}
