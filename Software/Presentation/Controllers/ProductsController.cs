using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;

namespace Presentation.Controllers
{
    public class ProductsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index()
        {
            //if (User.Identity.IsAuthenticated)
            //{
            //    var identity = (System.Security.Claims.ClaimsIdentity) User.Identity;
            //    string role = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            //    ViewBag.role = role;
            //}
            //else
            //{
            //    ViewBag.role = "noauthenticate";

            //}
            return View(UnitOfWork.ProductRepository.Get( ));
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = UnitOfWork.ProductRepository.GetById(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Create()
        {
            ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(), "Id", "Title");
            return View();
        }

        private CodeGenerator codeGenerator = new CodeGenerator();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                string newFilenameUrl = string.Empty;
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    newFilenameUrl = "/Uploads/Product/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    product.ImageUrl = newFilenameUrl;
                }
                #endregion

                ProductGroup productGroup = UnitOfWork.ProductGroupRepository.GetById(product.ProductGroupId);

                product.Code = codeGenerator.ReturnProductCode(productGroup.Code);
                UnitOfWork.ProductRepository.Insert(product);
                UnitOfWork.Save();

                return RedirectToAction("Index");
            }

            ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(), "Id", "Title");
            return View(product);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = UnitOfWork.ProductRepository.GetById(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(), "Id", "Title", product.ProductGroupId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase fileupload)
        {
            if (ModelState.IsValid)
            {
                #region Upload and resize image if needed
                string newFilenameUrl = string.Empty;
                if (fileupload != null)
                {
                    string filename = Path.GetFileName(fileupload.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(filename);

                    newFilenameUrl = "/Uploads/Product/" + newFilename;
                    string physicalFilename = Server.MapPath(newFilenameUrl);

                    fileupload.SaveAs(physicalFilename);

                    product.ImageUrl = newFilenameUrl;
                }
                #endregion
                UnitOfWork.ProductRepository.Update(product);
                UnitOfWork.Save();

                return RedirectToAction("Index");
            }
            ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(), "Id", "Title", product.ProductGroupId);
            return View(product);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = UnitOfWork.ProductRepository.GetById(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Product entity)
        {
            entity = UnitOfWork.ProductRepository.GetById(entity.Id);
            UnitOfWork.ProductRepository.Delete(entity);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
