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
    public class BranchUsersController : Infrastructure.BaseControllerWithUnitOfWork
    {

        public ActionResult Index(Guid id)
        {
            ViewBag.branchTitle = UnitOfWork.BranchRepository.GetById(id).Title;
            return View(UnitOfWork.BranchUserRepository.Get(current => current.BranchId == id));
        }


        public ActionResult Create(Guid id)
        {
            ViewBag.branchId = id;
            ViewBag.UserId = new SelectList(UnitOfWork.UserRepository.Get(c=>c.Role.Name.ToLower()== "branch" || c.Role.Name.ToLower()== "factory"), "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BranchUser branchUser, Guid id)
        {
            if (ModelState.IsValid)
            {
                branchUser.BranchId = id;
                UnitOfWork.BranchUserRepository.Insert(branchUser);
                UnitOfWork.Save();
                return RedirectToAction("Index", new { id = id });
            }

            ViewBag.branchId = id;
            ViewBag.UserId = new SelectList(UnitOfWork.UserRepository.Get(), "Id", "FullName");
            return View(branchUser);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchUser branchUser = UnitOfWork.BranchUserRepository.GetById(id.Value);
            if (branchUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.branchId = branchUser.BranchId;

            ViewBag.UserId = new SelectList(UnitOfWork.UserRepository.Get(), "Id", "FullName", branchUser.UserId);
            return View(branchUser);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BranchUser branchUser)
        {
            if (ModelState.IsValid)
            {
                UnitOfWork.BranchUserRepository.Update(branchUser);
                UnitOfWork.Save();
                return RedirectToAction("Index",new{id=branchUser.BranchId});
            }
            ViewBag.UserId = new SelectList(UnitOfWork.UserRepository.Get(), "Id", "FullName", branchUser.UserId);
            ViewBag.branchId = branchUser.BranchId;

            return View(branchUser);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BranchUser branchUser = UnitOfWork.BranchUserRepository.GetById(id.Value);
            if (branchUser == null)
            {
                return HttpNotFound();
            }
            ViewBag.branchId = branchUser.BranchId;

            return View(branchUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(BranchUser entity)
        {
            entity = UnitOfWork.BranchUserRepository.GetById(entity.Id);
            UnitOfWork.BranchUserRepository.Delete(entity);
            UnitOfWork.Save();
            return RedirectToAction("Index",new{id=entity.BranchId});
        }


    }
}
