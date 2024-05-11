
using LegendCR.CommonDefinitions.DTO;
using LegendCR.CommonDefinitions.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.CommonDefinitions.Responses
{
    public class ContactUsRequest:BaseRequest
    {
        public ContactUsDTO ContactUsDTO { get; set; }
    }
}
