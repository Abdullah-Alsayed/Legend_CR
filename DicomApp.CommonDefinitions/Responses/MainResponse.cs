using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;

namespace DicomApp.CommonDefinitions.Responses
{
    public class MainResponse
    {
        public List<AdsDTO> TopAdvertisements { get; set; } = new List<AdsDTO>();
        public List<AdsDTO> AllAdvertisements { get; set; } = new List<AdsDTO>();
        public List<GameDTO> Games { get; set; } = new List<GameDTO>();
    }
}
