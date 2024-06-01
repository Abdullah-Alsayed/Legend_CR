using DicomApp.CommonDefinitions.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.Requests
{
    public class Common_ResourceRequest : BaseRequest
    {
        public Common_ResourceDTO Common_ResourceRecord { get; set; }

    
}
}
