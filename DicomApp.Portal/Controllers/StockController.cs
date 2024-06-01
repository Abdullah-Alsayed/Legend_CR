
using DicomApp.CommonDefinitions.DTO.VendorProductDTOs;
using DicomApp.CommonDefinitions.Requests;
using DicomApp.DAL.DB;
using DicomApp.Helpers;
using DicomApp.CommonDefinitions.DTO;
using DicomApp.CommonDefinitions.Responses;
using DicomApp.Helpers;
using DicomApp.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore.Options;
using Rotativa.AspNetCore;
using System.Linq;
using System;
using DicomApp.BL.Services;

namespace DicomApp.Portal.Controllers
{
    public class StockController : Controller
    {
        private readonly ShippingDBContext _context;
        public StockController(ShippingDBContext context)
        {
            _context = context;
        }
    }
}
