using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LegendCR.CommonDefinitions.DTO
{
    public class StatusDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DeliveryManName { get; set; }
    }
}
