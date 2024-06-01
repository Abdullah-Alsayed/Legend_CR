using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DicomApp.CommonDefinitions.Responses
{
    
    public class BaseResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public int TotalCount { get; set; }
    }
}
