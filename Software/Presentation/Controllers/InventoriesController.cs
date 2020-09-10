using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Models;
using ViewModels;

namespace Presentation.Controllers
{
    public class InventoriesController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index(Guid id)
        {
            var inventories = UnitOfWork.InventoryRepository.Get(c=>c.BranchId==id).OrderByDescending(i => i.CreationDate);
            return View(inventories.ToList());
        }

        public ActionResult Create(Guid id)
        {
            ViewBag.BranchId = id;
            ViewBag.MattressId = new SelectList(UnitOfWork.MattressRepository.Get(current => current.IsActive), "Id", "Title");
            ViewBag.ProductColorId = new SelectList(UnitOfWork.ProductColorRepository.Get(current => current.IsActive), "Id", "Title");
            ViewBag.ProductId = new SelectList(UnitOfWork.ProductRepository.Get(current => current.IsActive), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inventory inventory, Guid id)
        {
            if (ModelState.IsValid)
            {
                Inventory oInventory = UnitOfWork.InventoryRepository.Get(current =>
                        current.ProductId == inventory.ProductId && current.BranchId == inventory.BranchId)
                    .FirstOrDefault();

                if (oInventory != null)
                {
                    ModelState.AddModelError("duplicate",
                        "موجودی این محصول قبلا در این شعبه به ثبت رسیده است و برای یک محصول در یک شعبه امکان ثبت دو موجودی وجود ندارد.");
                }
                else
                {
                    inventory.BranchId = id;
                    UnitOfWork.InventoryRepository.Insert(inventory);
                    UnitOfWork.Save();
                    return RedirectToAction("Index",new{id=id});
                }
            }

            ViewBag.BranchId = id;
            ViewBag.ProductId = new SelectList(UnitOfWork.ProductRepository.Get(current => current.IsActive), "Id", "Title");
           
            ViewBag.MattressId = new SelectList(UnitOfWork.MattressRepository.Get(current => current.IsActive), "Id", "Title");
            return View(inventory);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inventory inventory = UnitOfWork.InventoryRepository.GetById(id.Value);
            if (inventory == null)
            {
                return HttpNotFound();
            }

            ViewBag.ProductColorId = new SelectList(UnitOfWork.ProductColorRepository.Get(current =>current.ProductId==inventory.ProductId&& current.IsActive), "Id", "Title",inventory.ProductColorId);

            ViewBag.MattressId = new SelectList(UnitOfWork.MattressRepository.Get(current => current.IsActive), "Id", "Title",inventory.MattressId);
            ViewBag.BranchId = inventory.BranchId;
            ViewBag.ProductId = new SelectList(UnitOfWork.ProductRepository.Get(current => current.IsActive), "Id", "Title", inventory.ProductId);
            return View(inventory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                Inventory oInventory = UnitOfWork.InventoryRepository.Get(current => current.Id != inventory.Id &&
                        current.ProductId == inventory.ProductId && current.BranchId == inventory.BranchId)
                    .FirstOrDefault();

                if (oInventory != null)
                {
                    ModelState.AddModelError("duplicate",
                        "موجودی این محصول قبلا در این شعبه به ثبت رسیده است و برای یک محصول در یک شعبه امکان ثبت دو موجودی وجود ندارد.");
                }
                else
                {
                    inventory.IsDeleted = false;
                    UnitOfWork.InventoryRepository.Update(inventory);
                    UnitOfWork.Save();
                    return RedirectToAction("Index", new { id = inventory.BranchId });
                }
            }
            ViewBag.ProductColorId = new SelectList(UnitOfWork.ProductColorRepository.Get(current => current.ProductId == inventory.ProductId && current.IsActive), "Id", "Title", inventory.ProductColorId);

            ViewBag.MattressId = new SelectList(UnitOfWork.MattressRepository.Get(current => current.IsActive), "Id", "Title", inventory.MattressId);
            ViewBag.BranchId = inventory.BranchId;
            ViewBag.ProductId = new SelectList(UnitOfWork.ProductRepository.Get(current => current.IsActive), "Id", "Title", inventory.ProductId);
            return View(inventory);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Inventory inventory = UnitOfWork.InventoryRepository.GetById(id.Value);

            if (inventory == null)
            {
                return HttpNotFound();
            }

            return View(inventory);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Inventory entity)
        {
            entity = UnitOfWork.InventoryRepository.GetById(entity.Id);
            UnitOfWork.InventoryRepository.Delete(entity);
            UnitOfWork.Save();

            return RedirectToAction("Index", new { id = entity.BranchId });
        }


        public ActionResult GetProductColorByProductId(string id)
        {
            Guid productId = new Guid(id);
            var productColors = UnitOfWork.ProductColorRepository.Get(c => c.ProductId == productId).OrderBy(current => current.Title).ToList();
            List<CityItemViewModel> cityItems = new List<CityItemViewModel>();
            foreach (ProductColor productColor in productColors)
            {
                cityItems.Add(new CityItemViewModel()
                {
                    Text = productColor.Title,
                    Value = productColor.Id.ToString()
                });
            }
            return Json(cityItems, JsonRequestBehavior.AllowGet);
        }
    }
}
