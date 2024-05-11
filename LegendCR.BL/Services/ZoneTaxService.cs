using LegendCR.CommonDefinitions.DTO;
using LegendCR.DAL.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegendCR.BL.Services
{
    public static class ZoneTaxService
    {

        public static void Add(ZoneTaxDTO ZoneTaxDTO, ApplicationDBContext context)
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
