using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.Requests
{
    public class SeedPermissionRequest : BaseRequest
    {
        public Dictionary<string, List<string>> Permissions { get; set; }
    }
}
