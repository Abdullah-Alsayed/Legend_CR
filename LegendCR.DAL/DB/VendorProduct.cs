
using System;
using System.Collections.Generic;

namespace LegendCR.DAL.DB
{
    public partial class VendorProduct
    {
        public VendorProduct()
        {

        }

        public int VendorProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double OriginalPrice { get; set; }
        public int VendorId { get; set; }
        public string ImageUrl { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int AvailableStock { get; set; }
        public double StockPrice { get; set; }
        public string Size { get; set; }

        public virtual CommonUser Vendor { get; set; }

    }
}
