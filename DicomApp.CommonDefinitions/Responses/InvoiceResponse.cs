using System;
using System.Collections.Generic;
using System.Text;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.DTO.AdvertisementDTOs;

namespace DicomApp.CommonDefinitions.Responses
{
    public class InvoiceResponse : BaseResponse
    {
        public InvoiceDTO InvoiceDTO { get; set; }
        public List<InvoiceDTO> InvoiceDTOs { get; set; }
    }
}
