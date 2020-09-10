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
    public class FundsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Funds
        public ActionResult Index(Guid id)
        {
            var funds = db.Funds.Include(f => f.Branch).Where(f=>f.BranchId==id&& f.IsDeleted==false).OrderByDescending(f=>f.ReceiveDate);

            Branch branch = db.Branches.Find(id);

            ViewBag.branchTitle = branch.Title;
            return View(funds.ToList());
        }
        public DateTime GetGrDate(DateTime datetime)
        {
            System.Globalization.PersianCalendar c = new System.Globalization.PersianCalendar();

            DateTime date = c.ToDateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0, 0);

            return date;
        }
        public ActionResult Create(Guid id)
        {
            ViewBag.BranchId = id;
            return View();
        }

        // POST: Funds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Fund fund, Guid id)
        {
            if (ModelState.IsValid)
            {
                fund.FinishDate = null;
                fund.RemainAmount = fund.Amount;
                fund.ReceiveDate = GetGrDate(fund.ReceiveDate);
                fund.BranchId = id;
				fund.IsDeleted=false;
				fund.CreationDate= DateTime.Now; 
                fund.Id = Guid.NewGuid();
                db.Funds.Add(fund);
                db.SaveChanges();
                return RedirectToAction("Index",new{id=id});
            }

            ViewBag.BranchId = id;
            return View(fund);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fund fund = db.Funds.Find(id);
            if (fund == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchId = fund.BranchId;
            return View(fund);
        }

        // POST: Funds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Fund fund)
        {
            if (ModelState.IsValid)
            {
				fund.IsDeleted=false;
                db.Entry(fund).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new {id=fund.BranchId});
            }
            ViewBag.BranchId = fund.BranchId;
            return View(fund);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fund fund = db.Funds.Find(id);
            if (fund == null)
            {
                return HttpNotFound();
            }
            return View(fund);
        }

        // POST: Funds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Fund fund = db.Funds.Find(id);
			fund.IsDeleted=true;
			fund.DeletionDate=DateTime.Now;
 
            db.SaveChanges();
            return RedirectToAction("Index", new { id = fund.BranchId });
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
