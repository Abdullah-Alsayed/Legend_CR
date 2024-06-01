using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO;

namespace DicomApp.CommonDefinitions.Requests
{
    public class CategoryRequest : BaseRequest
    {
        public CategoryDTO CategoryDTO { get; set; }
    }
}
