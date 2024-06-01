using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class ShipmentType
    {
        public ShipmentType()
        {
            Shipment = new HashSet<Advertisement>();
        }

        public int ShipmentTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Advertisement> Shipment { get; set; }
    }
}
