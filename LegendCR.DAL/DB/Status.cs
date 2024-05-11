
using System;
using System.Collections.Generic;

namespace LegendCR.DAL.DB
{
    public partial class Status
    {
        public Status()
        {
   
        }

        public int Id { get; set; }
        public string Name { get; set; }

        //public virtual ICollection<Shipment> Shipment { get; set; }
    }
}
