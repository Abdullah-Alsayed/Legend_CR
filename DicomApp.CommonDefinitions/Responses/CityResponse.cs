
using DicomApp.CommonDefinitions.Records;
using DicomDB.CommonDefinitions.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.Responses
{
    public class CityResponse:BaseResponse
    {
       public List<CityDTO> CityDTOs { get; set; }
       public CityDTO CityDTO { get; set; }

    }
}
