using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.CommonDefinitions.DTO.ShipmentDTOs
{
    public class CustomerFollowUpDTO
    {
        public int CallAnswerCount { get; set; }
        public int CallNotAnswerCount { get; set; }

        public int ShipmentCustomerFollowUpId { get; set; }
        public int ShipmentId { get; set; }
        public string Note { get; set; }

        public bool? IsConfirmed { get; set; }
        public bool? IsCallAnswered { get; set; }
        public DateTime? NextCallOn { get; set; }
        public DateTime? NextCallTimeFrom { get; set; }
        public DateTime? NextCallTimeTo { get; set; }
    }
}
