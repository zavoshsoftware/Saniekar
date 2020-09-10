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
    public class MattressesController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Mattresses
        public ActionResult Index()
        {
            return View(db.Mattresses.Where(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: Mattresses/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mattress mattress = db.Mattresses.Find(id);
            if (mattress == null)
            {
                return HttpNotFound();
            }
            return View(mattress);
        }

        // GET: Mattresses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mattresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Mattress mattress)
        {
            if (ModelState.IsValid)
            {
				mattress.IsDeleted=false;
				mattress.CreationDate= DateTime.Now; 
                mattress.Id = Guid.NewGuid();
                db.Mattresses.Add(mattress);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mattress);
        }

        // GET: Mattresses/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mattress mattress = db.Mattresses.Find(id);
            if (mattress == null)
            {
                return HttpNotFound();
            }
            return View(mattress);
        }

        // POST: Mattresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,IsActive,CreationDate,LastModifiedDate,IsDeleted,DeletionDate,Description")] Mattress mattress)
        {
            if (ModelState.IsValid)
            {
				mattress.IsDeleted=false;
                db.Entry(mattress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mattress);
        }

        // GET: Mattresses/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mattress mattress = db.Mattresses.Find(id);
            if (mattress == null)
            {
                return HttpNotFound();
            }
            return View(mattress);
        }

        // POST: Mattresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Mattress mattress = db.Mattresses.Find(id);
			mattress.IsDeleted=true;
			mattress.DeletionDate=DateTime.Now;
 
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
