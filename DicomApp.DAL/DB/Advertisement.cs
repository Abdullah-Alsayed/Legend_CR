using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DicomApp.DAL.DB
{
    public partial class Advertisement
    {
        public Advertisement()
        {
            FollowUp = new HashSet<FollowUp>();
        }

        public int AdvertisementId { get; set; }
        public string RefId { get; set; }
        public int GameId { get; set; }
        public int StatusId { get; set; }

        [ForeignKey(nameof(Vendor))]
        public int VendorId { get; set; }

        [ForeignKey(nameof(Buyer))]
        public int? BuyerId { get; set; }

        public int? AdvertisementRequestId { get; set; }
        public int CashTransferId { get; set; }

        public string Description { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public int Price { get; set; }
        public bool IsRefund { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public int? LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Game Game { get; set; }
        public virtual Status Status { get; set; }

        public virtual CommonUser Vendor { get; set; }
        public virtual CommonUser Buyer { get; set; }

        public virtual CashTransfer CashTransfer { get; set; }
        public virtual ICollection<AdvertisementPhotos> AdvertisementPhotos { get; set; }
        public virtual ICollection<FollowUp> FollowUp { get; set; }
    }
}
