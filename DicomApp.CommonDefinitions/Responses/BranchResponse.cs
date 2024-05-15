using DicomApp.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.Responses
{
    public class BranchResponse : BaseResponse
    {

        public List<BranchDTO> BranchDTOs { get; set; }
    }
}
