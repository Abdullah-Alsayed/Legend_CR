namespace DicomApp.CommonDefinitions.DTO.AdvertisementDTOs
{
    public class ShipItemDTO
    {
        public int ShipmentItemId { get; set; }
        public int ShipmentId { get; set; }
        public int StatusId { get; set; }
        public StatusDTO StatusDTO { get; set; }
        public int? VendorProductId { get; set; }
        public bool IsStcok { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }

        public string ImageUrl { get; set; }
        public int CourierQuantityDelivered { get; set; }
        public int CourierQuantityReturned { get; set; }
    }
}
