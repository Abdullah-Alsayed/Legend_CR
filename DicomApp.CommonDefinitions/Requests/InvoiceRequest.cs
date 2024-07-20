using DicomApp.CommonDefinitions.DTO;

namespace DicomApp.CommonDefinitions.Requests
{
    public class InvoiceRequest : BaseRequest
    {
        public InvoiceDTO InvoiceDTO { get; set; } = new InvoiceDTO();
    }
}
