using LegendCR.CommonDefinitions.DTO;
using LegendCR.CommonDefinitions.DTO.ShipmentDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.CommonDefinitions.Requests
{
    public class DashboardRequest : BaseRequest
    {
        public int TopArea { get; set; }
        public int TopDriver { get; set; }
        public int TopAccount { get; set; }
        public ShipDTO ShipDTO { get; set; }
        public int PackagingStock { get; set; }
    }
}
