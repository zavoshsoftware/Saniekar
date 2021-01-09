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
    public class FundDetailsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: FundDetails
        public ActionResult Index(Guid id)
        {
            var fundDetails = db.FundDetails.Include(f => f.Fund).Where(f=>f.FundId==id&& f.IsDeleted==false).OrderByDescending(f=>f.CreationDate);
            return View(fundDetails.ToList());
        }

        public DateTime GetGrDate(DateTime datetime)
        {
            System.Globalization.PersianCalendar c = new System.Globalization.PersianCalendar();

            DateTime date = c.ToDateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0, 0);

            return date;
        }

        public ActionResult Create(Guid id)
        {
            ViewBag.FundId = id;
            return View();
        }

        // POST: FundDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FundDetail fundDetail,Guid id)
        {
            if (ModelState.IsValid)
            {
                Fund fund = db.Funds.Find(id);

                decimal remain = fund.RemainAmount - fundDetail.Amount;

                fundDetail.RemainAmount = remain;
                fund.RemainAmount = remain;
                fundDetail.IsActive = true;

                fundDetail.FundId = id;
				fundDetail.IsDeleted=false;
				fundDetail.CreationDate= DateTime.Now; 
                fundDetail.Id = Guid.NewGuid();
                db.FundDetails.Add(fundDetail);
                db.SaveChanges();
                return RedirectToAction("Index",new{id=id});
            }

            ViewBag.FundId = id;
            return View(fundDetail);
        }

        // GET: FundDetails/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundDetail fundDetail = db.FundDetails.Find(id);
            if (fundDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.FundId =fundDetail.FundId;
            return View(fundDetail);
        }

        // POST: FundDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FundDetail fundDetail)
        {
            if (ModelState.IsValid)
            {
				fundDetail.IsDeleted=false;
                db.Entry(fundDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new{id=fundDetail.FundId});
            }
            ViewBag.FundId = fundDetail.FundId;
            return View(fundDetail);
        }

        // GET: FundDetails/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundDetail fundDetail = db.FundDetails.Find(id);
            if (fundDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.FundId = fundDetail.FundId;
            return View(fundDetail);
        }

        // POST: FundDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            FundDetail fundDetail = db.FundDetails.Find(id);
			fundDetail.IsDeleted=true;
			fundDetail.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index", new { id = fundDetail.FundId });
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
