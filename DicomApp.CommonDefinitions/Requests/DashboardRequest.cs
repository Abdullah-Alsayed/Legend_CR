using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;

namespace DicomApp.CommonDefinitions.Requests
{
    public class DashboardRequest : BaseRequest
    {
        public int TopArea { get; set; }
        public int TopDriver { get; set; }
        public int TopAccount { get; set; }
        public AdsDTO AdsDTO { get; set; }
        public int PackagingStock { get; set; }
    }
}
