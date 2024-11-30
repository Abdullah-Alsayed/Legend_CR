using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO;

namespace DicomApp.CommonDefinitions.Responses
{
    public class FollowUpResponse : BaseResponse
    {
        public List<HistoryDTO> FollowUpDTOs { get; set; } = new List<HistoryDTO>();
    }
}
