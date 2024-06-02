using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO.AdvertisementDTOs
{
    public class AdsDTO_Filter
    {
        public int AdvertisementId { get; set; }
        public string RefId { get; set; }
        public int ShipmentTypeId { get; set; }
        public int StatusId { get; set; }
        public int? BranchId { get; set; }
        public int? DeliveryManId { get; set; }
        public int AreaId { get; set; }
        public int ZoneId { get; set; }

        public int? PickupRequestId { get; set; }
        public List<int> PickupItemsIDs { get; set; }
        public int VendorId { get; set; }
        public int? CustomerId { get; set; }
        public bool IsPartialDelivery { get; set; }
        public bool? IsConfirmed { get; set; }

        public bool IsAdmin { get; set; }
        public bool? IsCallAnswered { get; set; }
        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }
        public string CancelComment { get; set; }
        public bool? IsAfford { get; set; }
        public double? ShippingFeesPaid { get; set; }
        public List<int> AdvertisementIds { get; set; }
        public List<int> ShipmentItemIds { get; set; }
        public List<ShipItemDTO> ShipItemDTOs { get; set; }
        public int ProblemTypeID { get; set; }
        public string Note { get; set; }
    }
}
