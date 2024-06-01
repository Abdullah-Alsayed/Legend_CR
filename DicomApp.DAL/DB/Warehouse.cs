
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class Warehouse
    {
        public int WarehouseId { get; set; }
        public int BranchId { get; set; }
        public string Description { get; set; }
        public string WarehouseName { get; set; }
    }
}
