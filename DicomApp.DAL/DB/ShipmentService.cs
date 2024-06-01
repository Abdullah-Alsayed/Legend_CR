using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class ShipmentService
    {
        public ShipmentService()
        {
            Shipment = new HashSet<Advertisement>();
        }

        public int Id { get; set; }
        public string ServiceName { get; set; }

        public virtual ICollection<Advertisement> Shipment { get; set; }
    }
}
