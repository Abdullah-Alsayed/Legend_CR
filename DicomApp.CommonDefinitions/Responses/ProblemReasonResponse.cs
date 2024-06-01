
using DicomApp.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.Responses
{
    public class ProblemTypeResponse : BaseResponse
    {
        public List<ProblemTypeDTO> ProblemTypeDTOs { get; set; }
        public ProblemTypeDTO ProblemTypeDTO { get; set; }
    }
}