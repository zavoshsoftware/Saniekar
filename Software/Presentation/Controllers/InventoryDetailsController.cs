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
using ViewModels;

namespace Presentation.Controllers
{
    public class InventoryDetailsController : Infrastructure.BaseControllerWithUnitOfWork
    {

        public ActionResult Index(Guid id)
        {
            Inventory inventory = UnitOfWork.InventoryRepository.GetById(id);

            List<InventoryDetail> inventoryDetails = UnitOfWork.InventoryDetailRepository
                .Get(i => i.InventoryId == id && i.IsDeleted == false).Include(i => i.Inventory)
                .OrderByDescending(i => i.CreationDate).Include(i => i.InventoryDetailType).ToList();

            if (inventory != null)
            {
                string color = "-";
                if (inventory.ProductColorId != null)
                    color = " رنگ " + inventory.ProductColor.Title;

                string mattress = "-";
                if (inventory.MattressId != null)
                    mattress = inventory.Mattress.Title;

                ViewBag.productTitle = inventory.Product.Title;
                ViewBag.productColor = color;
                ViewBag.mattressTitle = mattress;

                ViewBag.branch = inventory.Branch.Title;
            }

            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

            ViewBag.roleName = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            return View(inventoryDetails);
        }

        //     public ActionResult Details(Guid? id)
        //     {
        //         if (id == null)
        //         {
        //             return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //         }
        //         InventoryDetail inventoryDetail = db.InventoryDetails.Find(id);
        //         if (inventoryDetail == null)
        //         {
        //             return HttpNotFound();
        //         }
        //         return View(inventoryDetail);
        //     }

        public ActionResult Create(Guid id)
        {
            Inventory inventory = UnitOfWork.InventoryRepository.GetById(id);


            string color = "-";
            if (inventory.ProductColorId != null)
                color = " رنگ " + inventory.ProductColor.Title;

            string mattress = "-";
            if (inventory.MattressId != null)
                mattress = inventory.Mattress.Title;

            InventoryDetailEditViewModel item = new InventoryDetailEditViewModel()
            {
                Id = id,
                Title = inventory.Product.Title,
                ColorTitle = color,
                MattressTitle = mattress,
            };
            ViewBag.inventoryId = id;

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InventoryDetailEditViewModel inventoryDetailViewModel, Guid id)
        {
            Inventory inventory = UnitOfWork.InventoryRepository.GetById(id);

            if (ModelState.IsValid)
            {
                InventoryDetailHelper helper = new InventoryDetailHelper();

                if (inventory.Stock + inventoryDetailViewModel.Quantity <= 0)
                {
                    ModelState.AddModelError("invalidQty",
                        "مجموع موجودی وارد شده و موجودی فعلی این انبار منفی می باشد.");
                }
                else
                {
                    InventoryDetail inventoryDetail = helper.Insert(id, "diffrent", inventoryDetailViewModel.Quantity, inventory.Stock, null, null);
                    inventory.Stock += inventoryDetailViewModel.Quantity;
                    UnitOfWork.InventoryDetailRepository.Insert(inventoryDetail);

                    UnitOfWork.Save();

                    return RedirectToAction("Index",new{id=id});
                }
            }

            InventoryDetailEditViewModel item = new InventoryDetailEditViewModel()
            {
                Id = id,
                Title = inventory.Product.Title,
                ColorTitle = inventory.ProductColor.Title,
                MattressTitle = inventory.Mattress.Title,
            };

            ViewBag.inventoryId = id;
            return View(item);
        }

        //public ActionResult Edit(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    InventoryDetail inventoryDetail = UnitOfWork.InventoryDetailRepository.GetById(id.Value);
        //    if (inventoryDetail == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    InventoryDetailEditViewModel inventoryDetailEdit=new InventoryDetailEditViewModel()
        //    {
        //        Id = inventoryDetail.Id,
        //        Title = inventoryDetail.Inventory.Product.Title,
        //        ColorTitle = inventoryDetail.Inventory.ProductColor.Title,
        //        MattressTitle = inventoryDetail.Inventory.Mattress.Title,
        //        Quantity = inventoryDetail.Quantity,
        //        OrderPoint = inventoryDetail.Inventory.OrderPoint,
        //        OldQuantity = inventoryDetail.Quantity,
        //    };

        //    return View(inventoryDetailEdit);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(InventoryDetailEditViewModel inventoryDetailEdit)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        InventoryDetail inventoryDetail = UnitOfWork.InventoryDetailRepository.GetById(inventoryDetailEdit.Id);

        //        if (inventoryDetailEdit.OldQuantity != inventoryDetailEdit.Quantity)
        //        {
        //            inventoryDetail.Quantity = inventoryDetailEdit.Quantity;

        //            int dif = inventoryDetailEdit.Quantity - inventoryDetailEdit.OldQuantity;

        //            Inventory inventory = UnitOfWork.InventoryRepository.GetById(inventoryDetail.InventoryId);
        //        }

        //        inventoryDetail.IsDeleted = false;
        //        db.Entry(inventoryDetail).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.InventoryId = new SelectList(db.Inventories, "Id", "Description", inventoryDetail.InventoryId);
        //    ViewBag.InventoryDetailTypeId = new SelectList(db.InventoryDetailTypes, "Id", "Title", inventoryDetail.InventoryDetailTypeId);
        //    return View(inventoryDetail);
        //}

        //     public ActionResult Delete(Guid? id)
        //     {
        //         if (id == null)
        //         {
        //             return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //         }
        //         InventoryDetail inventoryDetail = db.InventoryDetails.Find(id);
        //         if (inventoryDetail == null)
        //         {
        //             return HttpNotFound();
        //         }
        //         return View(inventoryDetail);
        //     }

        //     [HttpPost, ActionName("Delete")]
        //     [ValidateAntiForgeryToken]
        //     public ActionResult DeleteConfirmed(Guid id)
        //     {
        //         InventoryDetail inventoryDetail = db.InventoryDetails.Find(id);
        //inventoryDetail.IsDeleted=true;
        //inventoryDetail.DeletionDate=DateTime.Now;

        //         db.SaveChanges();
        //         return RedirectToAction("Index");
        //     }

        //     protected override void Dispose(bool disposing)
        //     {
        //         if (disposing)
        //         {
        //             db.Dispose();
        //         }
        //         base.Dispose(disposing);
        //     }
    }
}
