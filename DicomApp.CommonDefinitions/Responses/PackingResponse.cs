using System.Collections.Generic;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO;

namespace DicomApp.CommonDefinitions.Responses
{
    public class GameResponse : BaseResponse
    {
        public List<GameDTO> GameDTOs { get; set; }
        public GameDTO GameDTO { get; set; }
    }
}
