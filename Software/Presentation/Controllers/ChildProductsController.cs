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
    public class ChildProductsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        //public ActionResult Index(Guid id)
        //{
        //    ViewBag.Title = "مدیریت محصولات زیرمجموعه " + UnitOfWork.ProductRepository.GetById(id).Title;
        //    return View(UnitOfWork.ProductRepository.Get(current=>current.ParentId==id));
        //}

        //public ActionResult Create(Guid id)
        //{
        //    ViewBag.parentId = id;
        //    return View();
        //}

        //private CodeGenerator codeGenerator = new CodeGenerator();

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Product product, Guid id, HttpPostedFileBase fileupload)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        #region Upload and resize image if needed
        //        string newFilenameUrl = string.Empty;
        //        if (fileupload != null)
        //        {
        //            string filename = Path.GetFileName(fileupload.FileName);
        //            string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
        //                                 + Path.GetExtension(filename);

        //            newFilenameUrl = "/Uploads/Product/" + newFilename;
        //            string physicalFilename = Server.MapPath(newFilenameUrl);

        //            fileupload.SaveAs(physicalFilename);

        //            product.ImageUrl = newFilenameUrl;
        //        }
        //        #endregion

        //        Product parentProduct = UnitOfWork.ProductRepository.GetById(id);

        //        product.ParentId = id;
        //        product.ProductGroupId = parentProduct.ProductGroupId;
        //        product.Code = codeGenerator.ReturnChildProductCode(parentProduct);

        //        UnitOfWork.ProductRepository.Insert(product);
        //        UnitOfWork.Save();

        //        return RedirectToAction("Index",new{id=id});
        //    }

        //    ViewBag.parentId = id;
        //    return View(product);
        //}

        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Product product = UnitOfWork.ProductRepository.GetById(id.Value);
        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.parentId = product.ParentId;
        //    return View(product);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Product product, HttpPostedFileBase fileupload)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        #region Upload and resize image if needed
        //        string newFilenameUrl = string.Empty;
        //        if (fileupload != null)
        //        {
        //            string filename = Path.GetFileName(fileupload.FileName);
        //            string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
        //                                 + Path.GetExtension(filename);

        //            newFilenameUrl = "/Uploads/Product/" + newFilename;
        //            string physicalFilename = Server.MapPath(newFilenameUrl);

        //            fileupload.SaveAs(physicalFilename);

        //            product.ImageUrl = newFilenameUrl;
        //        }
        //        #endregion
        //        UnitOfWork.ProductRepository.Update(product);
        //        UnitOfWork.Save();

        //        return RedirectToAction("Index",new{id=product.ParentId});
        //    }
        //    ViewBag.parentId = product.ParentId;
        //    return View(product);
        //}

        //public ActionResult Delete(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Product product = UnitOfWork.ProductRepository.GetById(id.Value);
        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.parentId = product.ParentId;
        //    return View(product);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(Product entity)
        //{
        //    entity = UnitOfWork.ProductRepository.GetById(entity.Id);
        //    UnitOfWork.ProductRepository.Delete(entity);
        //    UnitOfWork.Save();
        //    return RedirectToAction("Index",new{id=entity.ParentId});
        //}
    }
}
