using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DAL;
using Models;
using ViewModels;

namespace Presentation.Controllers
{
    public class RegionsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index()
        {
            var regions = UnitOfWork.RegionRepository.Get().OrderByDescending(r => r.CreationDate);
            return View(regions.ToList());
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult GetRegionByCity(string id)
        {
            Guid cityId = new Guid(id);

            var regions = UnitOfWork.RegionRepository.Get(c => c.CityId == cityId).OrderBy(current => current.Title).ToList();

            List<RegionItemViewModel> regionItems = new List<RegionItemViewModel>();

            foreach (Region region in regions)
            {
                regionItems.Add(new RegionItemViewModel()
                {
                    Text = region.Title,
                    Value = region.Id.ToString(),
                    Amount = region.ShipmentAmount.ToString()
                });
            }

            return Json(regionItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
           // ViewBag.CityId = new SelectList(UnitOfWork.CityRepository.Get(), "Id", "Title");
            return View();
        }

        // POST: Regions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Region region)
        {
            region.CityId = new Guid("2c730dce-774d-4007-88a9-4acb1dd48cea");
            if (ModelState.IsValid)
            {
                UnitOfWork.RegionRepository.Insert(region);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

           // ViewBag.CityId = new SelectList(UnitOfWork.CityRepository.Get(), "Id", "Title", region.CityId);
            return View(region);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Region region = UnitOfWork.RegionRepository.GetById(id.Value);
            if (region == null)
            {
                return HttpNotFound();
            }
         //   ViewBag.CityId = new SelectList(UnitOfWork.CityRepository.Get(), "Id", "Title", region.CityId);
            return View(region);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Region region)
        {
            if (ModelState.IsValid)
            {
                region.IsDeleted = false;
                UnitOfWork.RegionRepository.Update(region);
                return RedirectToAction("Index");
            }
         //   ViewBag.CityId = new SelectList(UnitOfWork.CityRepository.Get(), "Id", "Title", region.CityId);
            return View(region);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Region region = UnitOfWork.RegionRepository.GetById(id.Value);
            if (region == null)
            {
                return HttpNotFound();
            }
            return View(region);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Region entity)
        {
            entity = UnitOfWork.RegionRepository.GetById(entity.Id);
            UnitOfWork.RegionRepository.Delete(entity);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
