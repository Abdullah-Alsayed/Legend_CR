using LegendCR.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace LegendCR.CommonDefinitions.Responses
{
    public class BranchResponse : BaseResponse
    {

        public List<BranchDTO> BranchDTOs { get; set; }
    }
}
