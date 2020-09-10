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
    public class BranchesController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index()
        {
            ViewBag.fullName = Helpers.GetUserInfo.GetUserFullName();
            return View(UnitOfWork.BranchRepository.Get());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = UnitOfWork.BranchRepository.GetById(id.Value);
            if (branch == null)
            {
                return HttpNotFound();
            }
            ViewBag.fullName = Helpers.GetUserInfo.GetUserFullName();
            return View(branch);
        }

        public ActionResult Create()
        {
            ViewBag.fullName = Helpers.GetUserInfo.GetUserFullName();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Branch branch)
        {
            if (ModelState.IsValid)
            {
                UnitOfWork.BranchRepository.Insert(branch);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.fullName = Helpers.GetUserInfo.GetUserFullName();

            return View(branch);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = UnitOfWork.BranchRepository.GetById(id.Value);
            if (branch == null)
            {
                return HttpNotFound();
            }
            ViewBag.fullName = Helpers.GetUserInfo.GetUserFullName();
            return View(branch);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Branch branch)
        {
            if (ModelState.IsValid)
            {
                UnitOfWork.BranchRepository.Update(branch);
                return RedirectToAction("Index");
            }
            ViewBag.fullName = Helpers.GetUserInfo.GetUserFullName();
            return View(branch);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = UnitOfWork.BranchRepository.GetById(id.Value);
            if (branch == null)
            {
                return HttpNotFound();
            }
            ViewBag.fullName = Helpers.GetUserInfo.GetUserFullName();
            return View(branch);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Branch entity)
        {
            entity = UnitOfWork.BranchRepository.GetById(entity.Id);
            UnitOfWork.BranchRepository.Delete(entity);
            UnitOfWork.Save();
            ViewBag.fullName = Helpers.GetUserInfo.GetUserFullName();
            return RedirectToAction("Index");
        }


        public ActionResult List()
        {
            ViewBag.fullName = Helpers.GetUserInfo.GetUserFullName();
            return View(UnitOfWork.BranchRepository.Get());
        }
        public ActionResult ListForAccount()
        {
            ViewBag.fullName = Helpers.GetUserInfo.GetUserFullName();
            return View(UnitOfWork.BranchRepository.Get());
        }
   public ActionResult ListForFunds()
        {
            ViewBag.fullName = Helpers.GetUserInfo.GetUserFullName();
            return View(UnitOfWork.BranchRepository.Get());
        }

    }
}
