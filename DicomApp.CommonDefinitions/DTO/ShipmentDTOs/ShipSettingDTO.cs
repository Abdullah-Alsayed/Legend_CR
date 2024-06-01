using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO.ShipmentDTOs
{
    public class ShipSettingDTO
    {
        public int? GameId { get; set; }
        public int? CategoryId { get; set; }
        public int? WarehouseGameId { get; set; }
        public int? WarehouseCategoryId { get; set; }
        public string GameName { get; set; }
        public string CategoryName { get; set; }
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
