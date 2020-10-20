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
    public class InventoryDetailTypesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: InventoryDetailTypes
        public ActionResult Index()
        {
            return View(db.InventoryDetailTypes.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: InventoryDetailTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryDetailType inventoryDetailType = db.InventoryDetailTypes.Find(id);
            if (inventoryDetailType == null)
            {
                return HttpNotFound();
            }
            return View(inventoryDetailType);
        }

        // GET: InventoryDetailTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InventoryDetailTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Name,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] InventoryDetailType inventoryDetailType)
        {
            if (ModelState.IsValid)
            {
				inventoryDetailType.IsDeleted=false;
				inventoryDetailType.CreationDate= DateTime.Now; 
                inventoryDetailType.Id = Guid.NewGuid();
                db.InventoryDetailTypes.Add(inventoryDetailType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inventoryDetailType);
        }

        // GET: InventoryDetailTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryDetailType inventoryDetailType = db.InventoryDetailTypes.Find(id);
            if (inventoryDetailType == null)
            {
                return HttpNotFound();
            }
            return View(inventoryDetailType);
        }

        // POST: InventoryDetailTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Name,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] InventoryDetailType inventoryDetailType)
        {
            if (ModelState.IsValid)
            {
				inventoryDetailType.IsDeleted=false;
                db.Entry(inventoryDetailType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inventoryDetailType);
        }

        // GET: InventoryDetailTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InventoryDetailType inventoryDetailType = db.InventoryDetailTypes.Find(id);
            if (inventoryDetailType == null)
            {
                return HttpNotFound();
            }
            return View(inventoryDetailType);
        }

        // POST: InventoryDetailTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            InventoryDetailType inventoryDetailType = db.InventoryDetailTypes.Find(id);
			inventoryDetailType.IsDeleted=true;
			inventoryDetailType.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
