using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO;

namespace DicomApp.CommonDefinitions.Requests
{
    public class GameRequest : BaseRequest
    {
        public GameDTO GameDTO { get; set; }
    }
}
