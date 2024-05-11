
using LegendCR.CommonDefinitions.DTO.PickupDTOs;
using System.Collections.Generic;

namespace LegendCR.CommonDefinitions.Responses
{
    public class PickUpRequestResponse : BaseResponse
    {
        public List<PickupDTO> PickupDTOs { get; set; }
        public List<PickupItemDTO> PickupItemDTOs { get; set; }
    }
}