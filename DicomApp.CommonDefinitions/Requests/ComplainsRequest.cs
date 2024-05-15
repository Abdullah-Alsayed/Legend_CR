using DicomApp.CommonDefinitions.Requests;
using DicomDB.CommonDefinitions.DTO;

namespace DicomDB.CommonDefinitions.Requests
{
    public class ComplainsRequest : BaseRequest
    {
        public ComplainsDTO ComplainsDTO { get; set; }
    }
}
