using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO;

namespace DicomApp.CommonDefinitions.Responses
{
    public class GameChargeResponse : BaseResponse
    {
        public List<GameChargeDTO> GameChargeDTOs { get; set; } = new List<GameChargeDTO>();
        public GameChargeDTO GameChargeDTO { get; set; }
    }
}
