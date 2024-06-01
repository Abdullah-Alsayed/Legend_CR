using Microsoft.AspNetCore.Http;
using System;

namespace DicomApp.CommonDefinitions.DTO.VendorProductDTOs
{
    public class VendorProductDTO
    {
        public int VendorProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double OriginalPrice { get; set; }
        public string ImageUrl { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public double StockPrice { get; set; }

        public int AvailableStock { get; set; }
        public string Size { get; set; }
        public IFormFile File { get; set; }


        //For Filter
        public string Search { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        //
        public int ShippedItems { get; set; }
        public int RemainingItems { get; set; }
        public int TotalItems { get; set; }
    }
}