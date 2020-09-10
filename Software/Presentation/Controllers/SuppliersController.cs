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
    public class SuppliersController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index()
        {
            return View(UnitOfWork.SupplierRepository.Get());
        }
        
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = UnitOfWork.SupplierRepository.GetById(id.Value);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }
        
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                UnitOfWork.SupplierRepository.Insert(supplier);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(supplier);
        }
        
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = UnitOfWork.SupplierRepository.GetById(id.Value);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                UnitOfWork.SupplierRepository.Update(supplier);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(supplier);
        }
        
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = UnitOfWork.SupplierRepository.GetById(id.Value);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Supplier entity)
        {
            entity = UnitOfWork.SupplierRepository.GetById(entity.Id);
            UnitOfWork.SupplierRepository.Delete(entity);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
