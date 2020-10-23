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
    public class InventoryDetailsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private DatabaseContext db = new DatabaseContext();

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

        //     public ActionResult Create()
        //     {
        //         ViewBag.InventoryId = new SelectList(db.Inventories, "Id", "Description");
        //         ViewBag.InventoryDetailTypeId = new SelectList(db.InventoryDetailTypes, "Id", "Title");
        //         return View();
        //     }

        //     [HttpPost]
        //     [ValidateAntiForgeryToken]
        //     public ActionResult Create(InventoryDetail inventoryDetail)
        //     {
        //         if (ModelState.IsValid)
        //         {
        //	inventoryDetail.IsDeleted=false;
        //	inventoryDetail.CreationDate= DateTime.Now; 
        //             inventoryDetail.Id = Guid.NewGuid();
        //             db.InventoryDetails.Add(inventoryDetail);
        //             db.SaveChanges();
        //             return RedirectToAction("Index");
        //         }

        //         ViewBag.InventoryId = new SelectList(db.Inventories, "Id", "Description", inventoryDetail.InventoryId);
        //         ViewBag.InventoryDetailTypeId = new SelectList(db.InventoryDetailTypes, "Id", "Title", inventoryDetail.InventoryDetailTypeId);
        //         return View(inventoryDetail);
        //     }

        //     public ActionResult Edit(Guid? id)
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
        //         ViewBag.InventoryId = new SelectList(db.Inventories, "Id", "Description", inventoryDetail.InventoryId);
        //         ViewBag.InventoryDetailTypeId = new SelectList(db.InventoryDetailTypes, "Id", "Title", inventoryDetail.InventoryDetailTypeId);
        //         return View(inventoryDetail);
        //     }

        //     [HttpPost]
        //     [ValidateAntiForgeryToken]
        //     public ActionResult Edit(InventoryDetail inventoryDetail)
        //     {
        //         if (ModelState.IsValid)
        //         {
        //	inventoryDetail.IsDeleted=false;
        //             db.Entry(inventoryDetail).State = EntityState.Modified;
        //             db.SaveChanges();
        //             return RedirectToAction("Index");
        //         }
        //         ViewBag.InventoryId = new SelectList(db.Inventories, "Id", "Description", inventoryDetail.InventoryId);
        //         ViewBag.InventoryDetailTypeId = new SelectList(db.InventoryDetailTypes, "Id", "Title", inventoryDetail.InventoryDetailTypeId);
        //         return View(inventoryDetail);
        //     }

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
