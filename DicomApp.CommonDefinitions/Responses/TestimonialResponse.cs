using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO;

namespace DicomApp.CommonDefinitions.Responses
{
    public class TestimonialResponse : BaseResponse
    {
        public List<TestimonialDTO> TestimonialDTOs { get; set; }
    }
}
