using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;

namespace DicomApp.CommonDefinitions.Responses
{
    public class GamerServiceResponse : BaseResponse
    {
        public ServiceDTO ServiceDTO { get; set; }
        public List<ServiceDTO> ServiceDTOs { get; set; }
        public List<SelectOptionDTO> SelectOption { get; set; }
    }
}
