using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Helpers;
using Models;
using ViewModels;

namespace Presentation.Controllers
{
    public class InventoriesController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index(Guid id)
        {
            Branch branch = UnitOfWork.BranchRepository.GetById(id);

            if (branch != null)
                ViewBag.Title = "موجودی انبار " + branch.Title;


            List<Inventory> inventories = UnitOfWork.InventoryRepository.Get(c => c.BranchId == id)
                .OrderByDescending(i => i.CreationDate).ToList();

            List<InventoryListViewModel> list=new List<InventoryListViewModel>();

            foreach (Inventory inventory in inventories)
            {
                int detailCount = UnitOfWork.InventoryDetailRepository.Get(c => c.InventoryId == inventory.Id).Count();

                bool hasDetail = detailCount > 1;

                string mattress = "-";
                if (inventory.MattressId != null)
                    mattress = inventory.Mattress.Title;

                string color = "-";
                if (inventory.ProductColorId != null)
                    color = inventory.ProductColor.Title;

                list.Add(new InventoryListViewModel()
                {
                    Id = inventory.Id,
                    Quantity = inventory.Stock,
                    Title = inventory.Product.Title,
                    ColorTitle = color,
                    MattressTitle = mattress,
                    HasDetail = hasDetail
                });
            }

            return View(list);
        }

        public ActionResult Create(Guid id)
        {
            ViewBag.BranchId = id;
            ViewBag.MattressId = new SelectList(UnitOfWork.MattressRepository.Get(current => current.IsActive), "Id", "Title");
            ViewBag.ProductColorId = new SelectList(UnitOfWork.ProductColorRepository.Get(current => current.IsActive), "Id", "Title");
            ViewBag.ProductId = new SelectList(UnitOfWork.ProductRepository.Get(current => current.IsActive), "Id", "Title");
            ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(current => current.IsActive), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inventory inventory, Guid id)
        {
            if (ModelState.IsValid)
            {
                Inventory oInventory = UnitOfWork.InventoryRepository.Get(current =>
                        current.ProductId == inventory.ProductId && current.BranchId == id &&
                        current.ProductColorId == inventory.ProductColorId && current.MattressId == inventory.MattressId)
                    .FirstOrDefault();

                if (oInventory != null)
                {
                    ModelState.AddModelError("duplicate",
                        "موجودی این محصول قبلا در این شعبه به ثبت رسیده است و برای یک محصول در یک شعبه امکان ثبت دو موجودی وجود ندارد.");
                }
                else if (!IsValidMattress(inventory.ProductId, inventory.MattressId))
                {
                    ModelState.AddModelError("requiredMattress",
                        "برای این محصول حتما باید تشک انتخاب شود.");
                }
                else
                {
                    inventory.BranchId = id;
                    UnitOfWork.InventoryRepository.Insert(inventory);

                    InventoryDetailHelper inventoryDetailHelper = new InventoryDetailHelper();
                 InventoryDetail inventoryDetail=   inventoryDetailHelper.Insert(inventory.Id, "start", inventory.Stock, 0, null, null);

                    UnitOfWork.InventoryDetailRepository.Insert(inventoryDetail);
                    UnitOfWork.Save();
                    return RedirectToAction("Index", new { id = id });
                }
            }

            ViewBag.BranchId = id;
            ViewBag.MattressId = new SelectList(UnitOfWork.MattressRepository.Get(current => current.IsActive), "Id", "Title");
            ViewBag.ProductColorId = new SelectList(UnitOfWork.ProductColorRepository.Get(current => current.IsActive), "Id", "Title");
            ViewBag.ProductId = new SelectList(UnitOfWork.ProductRepository.Get(current => current.IsActive), "Id", "Title");
            ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(current => current.IsActive), "Id", "Title");
            return View(inventory);
        }

        public bool IsValidMattress(Guid productId, Guid? mattressId)
        {
            Product product = UnitOfWork.ProductRepository.GetById(productId);

            if (product != null)
            {
                if (product.HasMattress && mattressId == null)
                    return false;
                else
                    return true;
            }

            return false;

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

            ViewBag.ProductColorId = new SelectList(UnitOfWork.ProductColorRepository.Get(current => current.ProductId == inventory.ProductId && current.IsActive), "Id", "Title", inventory.ProductColorId);

            ViewBag.MattressId = new SelectList(UnitOfWork.MattressRepository.Get(current => current.IsActive), "Id", "Title", inventory.MattressId);
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
            List<CityItemViewModel> items = new List<CityItemViewModel>();
            foreach (ProductColor productColor in productColors)
            {
                items.Add(new CityItemViewModel()
                {
                    Text = productColor.Title,
                    Value = productColor.Id.ToString()
                });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductByProductGroupId(string id)
        {
            Guid productGroupId = new Guid(id);
            var products = UnitOfWork.ProductRepository.Get(c => c.ProductGroupId == productGroupId).OrderBy(current => current.Title).ToList();
            List<CityItemViewModel> items = new List<CityItemViewModel>();
            foreach (Product product in products)
            {
                items.Add(new CityItemViewModel()
                {
                    Text = product.Title,
                    Value = product.Id.ToString()
                });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckHasMattress(string id)
        {
            Guid productId = new Guid(id);

            var product = UnitOfWork.ProductRepository.Get(c => c.Id == productId).FirstOrDefault();

            if (product != null)
                return Json(product.HasMattress, JsonRequestBehavior.AllowGet);

            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}
