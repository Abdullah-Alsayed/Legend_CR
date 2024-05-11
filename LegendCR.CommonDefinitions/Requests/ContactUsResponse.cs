using LegendCR.CommonDefinitions.DTO;
using LegendCR.CommonDefinitions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.CommonDefinitions.Requests
{
    public class ContactUsResponse:BaseResponse
    {
        public List<ContactUsDTO> ContactUsDTOs { get; set; }
     }
}

