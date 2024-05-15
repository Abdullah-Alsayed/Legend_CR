
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class ShipmentType
    {
        public ShipmentType()
        {
            Shipment = new HashSet<Shipment>();
        }

        public int ShipmentTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Shipment> Shipment { get; set; }
    }
}
