
using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class PickupRequestType
    {
        public PickupRequestType()
        {
            PickupRequest = new HashSet<PickupRequest>();
        }

        public int PickupRequestTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PickupRequest> PickupRequest { get; set; }
    }
}
