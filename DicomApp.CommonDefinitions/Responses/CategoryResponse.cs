using System.Collections.Generic;
using DicomApp.CommonDefinitions.DTO;

namespace DicomApp.CommonDefinitions.Responses
{
    public class CategoryResponse : BaseResponse
    {
        public List<CategoryDTO> CategoryDTOs { get; set; }
        public CategoryDTO CategoryDTO { get; set; }
    }
}
