using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO;

namespace DicomApp.CommonDefinitions.Requests
{
    public class FollowUpRequest : BaseRequest
    {
        public HistoryDTO FollowUpDTO { get; set; } = new HistoryDTO();
    }
}
