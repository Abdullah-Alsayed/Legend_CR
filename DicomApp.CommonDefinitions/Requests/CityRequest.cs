using DicomApp.CommonDefinitions.Records;
using DicomApp.CommonDefinitions.Requests;

namespace DicomDB.CommonDefinitions.Requests
{
    public class CityRequest : BaseRequest
    {
        public CityDTO CityDTO { get; set; }
    }
}
