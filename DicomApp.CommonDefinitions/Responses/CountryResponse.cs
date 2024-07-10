using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO;

namespace DicomApp.CommonDefinitions.Responses
{
    public class CountryResponse : BaseResponse
    {
        public List<CountryDTO> CountryDTOs { get; set; }
    }
}
