using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.DAL.DB
{
    public class AdvertisementPhotos
    {
        public int AdvertisementId { get; set; }
        public string Url { get; set; }

        public virtual Advertisement Advertisement { get; set; }
    }
}
