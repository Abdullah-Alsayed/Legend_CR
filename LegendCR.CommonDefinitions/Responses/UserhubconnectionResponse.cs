
using LegendCR.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace LegendCR.CommonDefinitions.Responses
{
    public class UserhubconnectionResponse : BaseResponse
    {

        public List<UserhubconnectionDTO> UserhubconnectionRecords { get; set; }
    }
}