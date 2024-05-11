
using LegendCR.CommonDefinitions.DTO.VendorProductDTOs;
using LegendCR.CommonDefinitions.Requests;
using LegendCR.DAL.DB;
using LegendCR.Helpers;
using LegendCR.CommonDefinitions.DTO;
using LegendCR.CommonDefinitions.Responses;
using LegendCR.Helpers;
using LegendCR.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore.Options;
using Rotativa.AspNetCore;
using System.Linq;
using System;
using LegendCR.BL.Services;

namespace LegendCR.Portal.Controllers
{
    public class StockController : Controller
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;
        }
    }
}
