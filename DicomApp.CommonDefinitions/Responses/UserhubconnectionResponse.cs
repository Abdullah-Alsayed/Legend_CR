
using DicomApp.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace DicomApp.CommonDefinitions.Responses
{
    public class UserhubconnectionResponse : BaseResponse
    {

        public List<UserhubconnectionDTO> UserhubconnectionRecords { get; set; }
    }
}