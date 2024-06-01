using System;
using System.Collections.Generic;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;

namespace DicomApp.CommonDefinitions.DTO.PickupDTOs
{
    public class PickupDTO
    {
        public int PickupRequestId { get; set; }
        public int PickupRequestTypeId { get; set; }
        public int? BranchId { get; set; }
        public double PickupFees { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }

        public string Notes { get; set; }
        public int PickupItemsCount { get; set; }
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }

        //Vendor Info
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorPhone { get; set; }
        public string VendorAddress { get; set; }
        public string VendorLocation { get; set; }
        public string VendorLandmark { get; set; }
        public int VendorFloor { get; set; }
        public int? VendorApartment { get; set; }
        public string RefId { get; set; }
        public int? DeliveryManId { get; set; }
        public string DeliveryManName { get; set; }
        public IEnumerable<PickupItemDTO> PickupItems { get; set; }

        // For Add/Edit/Search
        public bool SELECTED { get; set; }
        public string Search { get; set; }
        public List<int> StatusIDs { get; set; }
        public List<int> PickupIDs { get; set; }
        public List<int> PickupItemsIDs { get; set; }
        public IEnumerable<AdsDTO> Shipments { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime DateFrom { get; set; }

        //For Paramter
        public string ProductIDs { get; set; }
        public string ProductsQuantity { get; set; }
        public string ProductsPrice { get; set; }
        public string ShipmentIDs { get; set; }
        public bool IncludeReturns { get; set; }
    }
}
