
using LegendCR.CommonDefinitions.Records;
using DicomDB.CommonDefinitions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.CommonDefinitions.Responses
{
    public class CityResponse:BaseResponse
    {
       public List<CityDTO> CityDTOs { get; set; }
       public CityDTO CityDTO { get; set; }

    }
}
