using LegendCR.CommonDefinitions.DTO.CashDTOs;
using System;
using System.Collections.Generic;

namespace LegendCR.CommonDefinitions.DTO.ShipmentDTOs
{
    public class ShipDTO
    {
        public int ShipmentId { get; set; }
        public string RefId { get; set; }
        public int ShipmentTypeId { get; set; }
        public int ShipmentServiceId { get; set; }
        public string ShipmentServiceName { get; set; }
        public int StatusId { get; set; }
        public int BranchId { get; set; }

        public string BranchName { get; set; }
        public int? DeliveryManId { get; set; }
        public string DeliveryManName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public int NoOfItems { get; set; }
        public string Description { get; set; }
        public string CancelComment { get; set; }
        public bool IsCashReceived { get; set; }
        public int? CashTransferId { get; set; }
        public string CashTransferRefId { get; set; }
        public int ReturnCount { get; set; }
        public int? PickupRequestId { get; set; }
        public bool IsTripCompleted { get; set; }
        public bool IsCourierReturned { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ShipToRefundId { get; set; }
        public string ShipToRefundRefId { get; set; }
        public double? RefundCash { get; set; }
        public double? RefundFees { get; set; }
        public double? VendorBalance { get; set; }

        public CashTransferDTO CashTransferDTO { get; set; }
        public StatusDTO StatusDTO { get; set; }
        public ShipSettingDTO SettingDTO { get; set; }
        public CustomerDetailsDTO CustomerDetailsDTO { get; set; }
        public VendorDetailsDTO VendorDetailsDTO { get; set; }
        public FeesDetailsDTO FeesDetailsDTO { get; set; }
        public CustomerFollowUpDTO CustomerFollowUpDTO { get; set; }
        public IEnumerable<ShipProblemDTO> ProblemDTOs { get; set; }
        public IEnumerable<FollowUpDTO> FollowUpDTOs { get; set; }
        public IEnumerable<ShipItemDTO> ShipItemDTOs { get; set; }

        public IEnumerable<AccountTransactionDTO> TransactionDTOs { get; set; }



        // For Insert/Update Shipment
        public string PartialItems { get; set; }
        public string ProductIDs { get; set; }
        public string ProductsQuantity { get; set; }
        public string ProductsPrice { get; set; }
        public bool SELECTED { get; set; }

        // Filter
        public string Search { get; set; }
        public List<int> StatusIDs { get; set; }
        public List<int> ShipmentIDs { get; set; }
        public List<int> ShipmentItemsIDs { get; set; }
        public bool IsForReceiveReturns { get; set; }
        public bool IsForReceiveCash { get; set; }
        public bool IsForTransferCash { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public bool HasProblem { get; set; }
        public bool? HasPickup { get; set; }
        public bool HasRefunded { get; set; }

        public bool CanReturnToVendor { get; set; }
        //public List<UserDTO> Vendors { get; set; }
    }
}
