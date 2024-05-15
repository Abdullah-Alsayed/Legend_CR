
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.Responses
{
    public class ContactUsRequest:BaseRequest
    {
        public ContactUsDTO ContactUsDTO { get; set; }
    }
}
