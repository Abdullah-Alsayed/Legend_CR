using System;
using System.Collections.Generic;

namespace DicomApp.DAL.DB
{
    public partial class Status
    {
        public Status()
        {
            Advertisements = new HashSet<Advertisement>();
            GamerServices = new HashSet<GamerService>();
        }

        public int Id { get; set; }

        public string NameEN { get; set; }
        public string NameAR { get; set; }

        public int StatusType { get; set; }

        public virtual ICollection<GamerService> GamerServices { get; set; }
        public virtual ICollection<Advertisement> Advertisements { get; set; }
    }
}
