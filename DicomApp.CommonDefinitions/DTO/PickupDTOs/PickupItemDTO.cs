﻿using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.DAL.DB;

namespace DicomApp.CommonDefinitions.DTO.PickupDTOs
{
    public class PickupItemDTO
    {
        public int PickupRequestItemId { get; set; }
        public int PickupRequestId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int? VendorProductId { get; set; }
        public string VendorProductName { get; set; }
        public int? AdvertisementId { get; set; }
        public string RefId { get; set; }

        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
