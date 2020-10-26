using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;

namespace Presentation.Controllers
{
    public class UsersController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index(string userType)
        {
            List<User> users = UnitOfWork.UserRepository.Get().ToList();

            if (userType != null)
                users = users.Where(c => c.Role.Name.ToLower() == "customer").ToList();
            else
                users = users.Where(c => c.Role.Name.ToLower() != "customer").ToList();

            return View(users);
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = UnitOfWork.UserRepository.GetById(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(UnitOfWork.RoleRepository.Get(), "Id", "Title");
            ViewBag.BranchId = new SelectList(UnitOfWork.BranchRepository.Get(), "Id", "Title");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                User factoryUser = UnitOfWork.UserRepository.Get(c => c.Role.Name == "factory").FirstOrDefault();

                if (factoryUser != null && user.RoleId == new Guid("F1DCEDB2-A865-4C73-BC51-1AFD28118D39"))
                {
                    ModelState.AddModelError("duplicate",
                        "قبلا کاربری با نقش کارخانه ثبت شده است.");

                }
                else
                {
                    CodeGenerator codeGenerator = new CodeGenerator();
                    user.Code = codeGenerator.ReturnUserCode();
                    UnitOfWork.UserRepository.Insert(user);
                    UnitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.BranchId = new SelectList(UnitOfWork.BranchRepository.Get(), "Id", "Title", user.BranchId);

            ViewBag.RoleId = new SelectList(UnitOfWork.RoleRepository.Get(), "Id", "Title", user.RoleId);
            return View(user);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = UnitOfWork.UserRepository.GetById(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.BranchId = new SelectList(UnitOfWork.BranchRepository.Get(), "Id", "Title", user.BranchId);
            ViewBag.RoleId = new SelectList(UnitOfWork.RoleRepository.Get(), "Id", "Title", user.RoleId);
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                User factoryUser = UnitOfWork.UserRepository.Get(c => c.Role.Name == "factory" && c.Id != user.Id)
                    .FirstOrDefault();

                if (factoryUser != null && user.RoleId == new Guid("F1DCEDB2-A865-4C73-BC51-1AFD28118D39"))
                {
                    ModelState.AddModelError("duplicate",
                        "قبلا کاربری با نقش کارخانه ثبت شده است.");
                }
                else
                {
                    UnitOfWork.UserRepository.Update(user);
                    UnitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.BranchId = new SelectList(UnitOfWork.BranchRepository.Get(), "Id", "Title", user.BranchId);
            ViewBag.RoleId = new SelectList(UnitOfWork.RoleRepository.Get(), "Id", "Title", user.RoleId);
            return View(user);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = UnitOfWork.UserRepository.GetById(id.Value);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(User entity)
        {
            entity = UnitOfWork.UserRepository.GetById(entity.Id);
            UnitOfWork.UserRepository.Delete(entity);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        }


        //public string UpdateCompanyUsers()
        //{
        //    List<BranchUser> companyUsers = UnitOfWork.BranchUserRepository.Get().ToList();

        //    foreach (BranchUser companyUser in companyUsers)
        //    {
        //        User user = UnitOfWork.UserRepository.GetById(companyUser.UserId);

        //        if (user != null)
        //        {
        //            user.BranchId = companyUser.BranchId;
        //            user.LastModifiedDate = DateTime.Now;
        //        }
        //    }

        //   UnitOfWork.Save();
            
        //    return String.Empty;
        //}
    }
}
