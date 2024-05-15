using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO.ShipmentDTOs
{
    public class ShipSettingDTO
    {
        public int? PackingId { get; set; }
        public int? PackingTypeId { get; set; }
        public int? WarehousePackingId { get; set; }
        public int? WarehousePackingTypeId { get; set; }
        public string PackingName { get; set; }
        public string PackingTypeName { get; set; }
        public int Weight { get; set; }
        public int WarehouseWeight { get; set; }

        public string Size { get; set; }
        public string WarehouseSize { get; set; }
        public bool IsPartialDelivery { get; set; }
        public bool IsStock { get; set; }
        public bool IsOpenPackage { get; set; }
        public bool IsFragilePackage { get; set; }
    }
}