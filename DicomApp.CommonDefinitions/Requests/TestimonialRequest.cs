using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO;

namespace DicomApp.CommonDefinitions.Requests
{
    public class TestimonialRequest : BaseRequest
    {
        public TestimonialDTO TestimonialDTO { get; set; }
    }
}
