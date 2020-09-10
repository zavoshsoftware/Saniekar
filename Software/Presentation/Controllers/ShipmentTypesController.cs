using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Presentation.Controllers
{
    public class ShipmentTypesController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index()
        {
            return View(UnitOfWork.ShipmentTypeRepository.Get().ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentType shipmentType = UnitOfWork.ShipmentTypeRepository.GetById(id.Value);

            if (shipmentType == null)
            {
                return HttpNotFound();
            }
            return View(shipmentType);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ShipmentType shipmentType)
        {
            if (ModelState.IsValid)
            {
                UnitOfWork.ShipmentTypeRepository.Insert(shipmentType);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(shipmentType);
        }

        // GET: ShipmentTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentType shipmentType = UnitOfWork.ShipmentTypeRepository.GetById(id.Value);
            if (shipmentType == null)
            {
                return HttpNotFound();
            }
            return View(shipmentType);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShipmentType shipmentType)
        {
            if (ModelState.IsValid)
            {
                shipmentType.IsDeleted = false;
                UnitOfWork.ShipmentTypeRepository.Update(shipmentType);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(shipmentType);
        }

        // GET: ShipmentTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShipmentType shipmentType = UnitOfWork.ShipmentTypeRepository.GetById(id.Value);
            if (shipmentType == null)
            {
                return HttpNotFound();
            }
            return View(shipmentType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(ShipmentType entity)
        {
            entity = UnitOfWork.ShipmentTypeRepository.GetById(entity.Id);
            UnitOfWork.ShipmentTypeRepository.Delete(entity);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        }
         
    }
}
