
using DicomApp.DAL.DB;
using DicomApp.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DicomApp.Portal.Controllers
{
    public class CityController : Controller
    {
        private readonly ShippingDBContext _context;

        //
        // GET: /City/

        public CityController(ShippingDBContext context)
        {
            _context = context;
        }

        [AuthorizePerRole("AreaList")]
        public ActionResult Index()
        {
            return View(_context.City.ToList());
        }
        //
        // GET: /City/Details/5
        [AuthorizePerRole("AreaDetails")]
        public ActionResult Details(int id = 0)
        {
            City city = _context.City.Find(id);
            if (city == null)
            {
                return View();
            }
            return View(city);
        }

        //
        // GET: /City/Create
        [AuthorizePerRole("AddArea")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /City/Create
        [AuthorizePerRole("AddArea")]
        [HttpPost]
        public ActionResult Create(City city)
        {
            if (ModelState.IsValid)
            {
                city.LastModifiedAt = DateTime.Now;
                city.CreatedAt = DateTime.Now;
                city.CreatedBy = "1";
                city.LastModifiedBy = "1";
                city.StateId = 1;
                city.LastModifiedAt = DateTime.Now;


                _context.City.Add(city);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(city);
        }

        //
        // GET: /City/Edit/5
        [AuthorizePerRole("EditArea")]

        public ActionResult Edit(int id = 0)
        {
            City city = _context.City.Find(id);
            if (city == null)
            {
                return View();
            }
            return View(city);
        }

        //
        // POST: /City/Edit/5

        [HttpPost]
        [AuthorizePerRole("EditArea")]
        public ActionResult Edit(City city)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(city).State = EntityState.Modified;
                city.LastModifiedAt = DateTime.Now;
                city.CreatedAt = DateTime.Now;
                city.CreatedBy = "1";
                city.LastModifiedBy = "1";
                city.StateId = 1;
                city.LastModifiedAt = DateTime.Now;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(city);
        }

        //
        // GET: /City/Delete/5

        public ActionResult Delete(int id = 0)
        {
            City city = _context.City.Find(id);
            if (city == null)
            {
                return View();
            }
            return View(city);
        }

        //
        // POST: /City/Delete/5
        [HttpPost, ActionName("DeleteArea")]
        [AuthorizePerRole("EditArea")]
        public ActionResult DeleteConfirmed(int id)
        {
            City city = _context.City.Find(id);
            _context.City.Remove(city);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}