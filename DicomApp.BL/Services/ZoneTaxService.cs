using DicomApp.CommonDefinitions.DTO;
using DicomApp.DAL.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DicomApp.BL.Services
{
    public static class ZoneTaxService
    {

        public static void Add(ZoneTaxDTO ZoneTaxDTO, ShippingDBContext context)
        {
            context.ZoneTax.Add(new ZoneTax
            {
               
                ZoneId = ZoneTaxDTO.ZoneId,
                Tax = ZoneTaxDTO.Tax,
                CreatedAt = ZoneTaxDTO.CreatedAt,
                LastModifiedAt = ZoneTaxDTO.LastModifiedAt,
                CreatedBy = ZoneTaxDTO.CreatedBy,
                LastModifiedBy = ZoneTaxDTO.LastModifiedBy,
           });
        }
    }
}
