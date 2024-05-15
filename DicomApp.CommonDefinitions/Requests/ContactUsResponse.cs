using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.Requests
{
    public class ContactUsResponse:BaseResponse
    {
        public List<ContactUsDTO> ContactUsDTOs { get; set; }
     }
}

