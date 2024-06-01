
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class ShipmentService
    {
        public ShipmentService()
        {
            Shipment = new HashSet<Shipment>();
        }

        public int Id { get; set; }
        public string ServiceName { get; set; }

        public virtual ICollection<Shipment> Shipment { get; set; }
    }
}
