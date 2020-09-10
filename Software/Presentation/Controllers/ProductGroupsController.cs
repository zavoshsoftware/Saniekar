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
    public class ProductGroupsController : Infrastructure.BaseControllerWithUnitOfWork
    {

        public ActionResult Index()
        {
            return View(UnitOfWork.ProductGroupRepository.Get());
        }


        public ActionResult Create()
        {
            return View();
        }

        private CodeGenerator codeGenerator = new CodeGenerator();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductGroup productGroup)
        {
            if (ModelState.IsValid)
            {
                productGroup.Code = codeGenerator.ReturnProductGroupCode().ToString();

                UnitOfWork.ProductGroupRepository.Insert(productGroup);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(productGroup);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = UnitOfWork.ProductGroupRepository.GetById(id.Value);

            if (productGroup == null)
            {
                return HttpNotFound();
            }

            return View(productGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductGroup productGroup)
        {
            if (ModelState.IsValid)
            {
                UnitOfWork.ProductGroupRepository.Update(productGroup);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(productGroup);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ProductGroup productGroup = UnitOfWork.ProductGroupRepository.GetById(id.Value);

            if (productGroup == null)
            {
                return HttpNotFound();
            }
            return View(productGroup);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(ProductGroup entity)
        {
            entity = UnitOfWork.ProductGroupRepository.GetById(entity.Id);
            UnitOfWork.ProductGroupRepository.Delete(entity);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}

