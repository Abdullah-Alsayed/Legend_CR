
using DicomApp.CommonDefinitions.Responses;
using DicomDB.CommonDefinitions.DTO;
using System.Collections.Generic;

namespace DicomDB.CommonDefinitions.Responses
{
    public class ComplainsResponse : BaseResponse
    {

        public List<ComplainsDTO> ComplainsDTOs { get; set; }
    }
}