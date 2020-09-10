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
    public class ProductColorsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: ProductColors
        public ActionResult Index(Guid id)
        {
            var productColors = db.ProductColors.Include(p => p.Product).Where(p=>p.ProductId==id&& p.IsDeleted==false).OrderByDescending(p=>p.CreationDate);
            return View(productColors.ToList());
        }

         
        public ActionResult Create(Guid id)
        {
            ViewBag.ProductId = id;
            return View();
        }

        // POST: ProductColors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductColor productColor,Guid id)
        {
            if (ModelState.IsValid)
            {
				productColor.IsDeleted=false;
				productColor.CreationDate= DateTime.Now; 
                productColor.Id = Guid.NewGuid();
                productColor.ProductId = id;
                db.ProductColors.Add(productColor);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.ProductId = id;
            return View(productColor);
        }
         
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductColor productColor = db.ProductColors.Find(id);
            if (productColor == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = productColor.ProductId ;
            return View(productColor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductColor productColor)
        {
            if (ModelState.IsValid)
            {
				productColor.IsDeleted=false;
                db.Entry(productColor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new{id=productColor.ProductId});
            }
            ViewBag.ProductId = productColor.ProductId;
            return View(productColor);
        }

        // GET: ProductColors/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductColor productColor = db.ProductColors.Find(id);
            if (productColor == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = productColor.ProductId;

            return View(productColor);
        }

        // POST: ProductColors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductColor productColor = db.ProductColors.Find(id);
			productColor.IsDeleted=true;
			productColor.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index", new { id = productColor.ProductId });
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
