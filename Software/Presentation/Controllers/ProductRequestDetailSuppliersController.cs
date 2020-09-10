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
    public class ProductRequestDetailSuppliersController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private DatabaseContext db = new DatabaseContext();


        private Guid _factoryBranchId = new Guid("31DF3AE2-5616-4620-8584-175BAAE43159");

        public ActionResult Index(Guid id)
        {
            var productRequestDetailSuppliers = db.ProductRequestDetailSuppliers.Include(p => p.Branch).Include(p => p.ProductRequestDetail).Where(p => p.ProductRequestDetailId == id && p.IsDeleted == false).OrderByDescending(p => p.CreationDate);
            return View(productRequestDetailSuppliers.ToList());
        }


        public ActionResult Create(Guid id)
        {
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Title");
            ViewBag.ProductRequestDetailId = id;
            return View();
        }

        // POST: ProductRequestDetailSuppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductRequestDetailSupplier productRequestDetailSupplier, Guid id)
        {
            if (ModelState.IsValid)
            {
                ProductRequestDetail productRequestDetail = UnitOfWork.ProductRequestDetailRepository.GetById(id);

                if (productRequestDetail.TotalSupplied + productRequestDetailSupplier.Quantity >
                    productRequestDetail.Quantity)
                {
                    ModelState.AddModelError("moreThanRequest",
                        "مقدار وارد شده برای محصول بیشتر از مقدار درخواستی می باشد می باشد.");
                    ViewBag.BranchId = new SelectList(db.Branches, "Id", "Title");
                    ViewBag.ProductRequestDetailId = id;
                    return View();
                }


                productRequestDetail.TotalSupplied += productRequestDetailSupplier.Quantity;
                UnitOfWork.ProductRequestDetailRepository.Update(productRequestDetail);


                Inventory inventory = UnitOfWork.InventoryRepository.Get(c =>
                        c.BranchId == _factoryBranchId && c.ProductId == productRequestDetail.ProductId &&
                        c.ProductColorId == productRequestDetail.ProductColorId &&
                        c.MattressId == productRequestDetail.MattressId)
                    .FirstOrDefault();

                if (inventory != null)
                {
                    inventory.Stock = inventory.Stock - productRequestDetailSupplier.Quantity;
                    UnitOfWork.InventoryRepository.Update(inventory);



                    productRequestDetailSupplier.BranchId = _factoryBranchId;
                    productRequestDetailSupplier.ProductRequestDetailId = id;
                    productRequestDetailSupplier.IsDeleted = false;
                    productRequestDetailSupplier.CreationDate = DateTime.Now;
                    productRequestDetailSupplier.Id = Guid.NewGuid();

                    UnitOfWork.ProductRequestDetailSupplierRepository.Insert(productRequestDetailSupplier);
                    UnitOfWork.Save();

                    return RedirectToAction("Index", new { id = id });
                }
                else
                {
                    ModelState.AddModelError("moreThanStock",
                        "مقدار وارد شده برای محصول بیشتر از موجودی انبار کارخانه می باشد.");
                    ViewBag.BranchId = new SelectList(db.Branches, "Id", "Title");
                    ViewBag.ProductRequestDetailId = id;
                    return View();
                }
            }

            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Title", productRequestDetailSupplier.BranchId);
            ViewBag.ProductRequestDetailId = new SelectList(db.ProductRequestDetails, "Id", "Description", productRequestDetailSupplier.ProductRequestDetailId);
            return View(productRequestDetailSupplier);
        }


        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductRequestDetailSupplier branch = UnitOfWork.ProductRequestDetailSupplierRepository.GetById(id.Value);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(ProductRequestDetailSupplier entity)
        {
            entity = UnitOfWork.ProductRequestDetailSupplierRepository.GetById(entity.Id);
            UnitOfWork.ProductRequestDetailSupplierRepository.Delete(entity);


            ProductRequestDetail productRequestDetail = UnitOfWork.ProductRequestDetailRepository.GetById(entity.ProductRequestDetailId);

            productRequestDetail.TotalSupplied -= entity.Quantity;
            UnitOfWork.ProductRequestDetailRepository.Update(productRequestDetail);


            Inventory inventory = UnitOfWork.InventoryRepository.Get(c =>
                    c.BranchId == _factoryBranchId && c.ProductId == productRequestDetail.ProductId &&
                    c.ProductColorId == productRequestDetail.ProductColorId &&
                    c.MattressId == productRequestDetail.MattressId)
                .FirstOrDefault();

            if (inventory != null)
            {
                inventory.Stock = inventory.Stock + entity.Quantity;
                UnitOfWork.InventoryRepository.Update(inventory);
            }


            UnitOfWork.Save();


            UnitOfWork.Save();
            return RedirectToAction("Index", new { id = entity.ProductRequestDetailId });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public List<Branch> GetUserBranches(Guid userId)
        {
            List<Branch> branches = new List<Branch>();
            List<BranchUser> branchUsers = UnitOfWork.BranchUserRepository
                .Get(current => current.UserId == userId).ToList();

            foreach (BranchUser branchUser in branchUsers)
            {
                branches.Add(branchUser.Branch);
            }

            return branches;

        }

        public ActionResult IndexForApprove()
        {
            List<ProductRequestRecieveViewModel> list = new List<ProductRequestRecieveViewModel>();


            if (User.Identity.IsAuthenticated)
            {
                var identity = (System.Security.Claims.ClaimsIdentity) User.Identity;

                User user = UnitOfWork.UserRepository.Get(current => current.CellNum == identity.Name)
                    .FirstOrDefault();

                if (user != null)
                {
                  Branch branch = GetUserBranches(user.Id).FirstOrDefault();

                    if (branch != null)
                    {
                        Guid branchId = branch.Id;

                        List<ProductRequestDetailSupplier> productRequestDetailSuppliers =
                            UnitOfWork.ProductRequestDetailSupplierRepository.Get(c=>c.BranchId==branchId).OrderBy(c => c.IsReceived).ToList();


                        foreach (ProductRequestDetailSupplier requestDetailSupplier in productRequestDetailSuppliers)
                        {
                            ProductRequestDetail prd =
                                UnitOfWork.ProductRequestDetailRepository.GetById(requestDetailSupplier.ProductRequestDetailId);


                            Product product =
                                UnitOfWork.ProductRepository.GetById(prd.ProductId);

                            string colorTitle = "-";

                            if (prd.ProductColorId != null)
                            {
                                ProductColor productColor =
                                    UnitOfWork.ProductColorRepository.GetById(prd.ProductColorId.Value);

                                if (productColor != null)
                                    colorTitle = productColor.Title;
                            }

                            string mattressTitle = "-";
                            if (prd.MattressId != null)
                            {
                                Mattress mattress =
                                    UnitOfWork.MattressRepository.GetById(prd.MattressId.Value);

                                if (mattress != null)
                                    mattressTitle = mattress.Title;
                            }

                            list.Add(new ProductRequestRecieveViewModel()
                            {
                                Quantity = requestDetailSupplier.Quantity,
                                CreationDateStr = requestDetailSupplier.CreationDateStr,
                                Id = requestDetailSupplier.Id,
                                IsRecieved = requestDetailSupplier.IsReceived,
                                BranchTitle = GetBranchTitleByReqDetailId(requestDetailSupplier.Id, prd),
                                ProductTitle = product.Title,
                                MattressTitle = mattressTitle,
                                ColorTitle = colorTitle

                            });
                        }

                    }
                }
            }



          
            return View(list);
        }

        public string GetBranchTitleByReqDetailId(Guid id, ProductRequestDetail prd)
        {
            ProductRequestDetailSupplier prds = UnitOfWork.ProductRequestDetailSupplierRepository.GetById(id);

            ProductRequest productRequest =
                UnitOfWork.ProductRequestRepository.GetById(prd.ProductRequestId);

            Branch branch = UnitOfWork.BranchRepository.GetById(productRequest.RequestBranchId.Value);

            return branch.Title;
        }


        public ActionResult Approve(Guid id)
        {
            ProductRequestDetailSupplier prds = UnitOfWork.ProductRequestDetailSupplierRepository.GetById(id);

            if (prds != null)
            {
                prds.IsReceived = true;
                UnitOfWork.ProductRequestDetailSupplierRepository.Update(prds);


                ProductRequestDetail prd =
                    UnitOfWork.ProductRequestDetailRepository.GetById(prds.ProductRequestDetailId);

                Product product = UnitOfWork.ProductRepository.GetById(prd.ProductId);

                decimal amount = product.Amount.Value;

                if (prd.ProductColorId != null)
                {
                    ProductColor productColor = UnitOfWork.ProductColorRepository.GetById(prd.ProductColorId.Value);
                    amount = amount + productColor.Amount;
                }

                ProductRequest productRequest = UnitOfWork.ProductRequestRepository.GetById(prd.ProductRequestId);

                InsertToAccount(amount, productRequest.RequestBranchId.Value, productRequest.Code,id);

                UnitOfWork.Save();
            }

            return RedirectToAction("IndexForApprove");
        }

        public ActionResult Notapprove(Guid id)
        {
            ProductRequestDetailSupplier prds = UnitOfWork.ProductRequestDetailSupplierRepository.GetById(id);

            if (prds != null)
            {
                prds.IsReceived = false;
                UnitOfWork.ProductRequestDetailSupplierRepository.Update(prds);

                List<Models.Account> accounts = UnitOfWork.AccountRepository.Get(c => c.RefrenceId == id).ToList();

                foreach (Account account in accounts)
                {
                    UnitOfWork.AccountRepository.Delete(account);
                }
                UnitOfWork.Save();
            }

            return RedirectToAction("IndexForApprove");
        }

        public void InsertToAccount(decimal totalAmount,  Guid branchId, int orderCode,Guid refrenceId)
        {
            Guid factoryBranchId=new Guid("31DF3AE2-5616-4620-8584-175BAAE43159");

            CodeGenerator codeGenerator = new CodeGenerator();


            int code = codeGenerator.ReturnAccountCode();

            Account account = new Account()
            {
                BranchId = factoryBranchId,
                Code = code,
                Bedehkar = 0,
                Bestankar = totalAmount,
                Body = "درخواست کالای کد " + orderCode,
                IsActive = true,
                Remain = totalAmount,
                RefrenceId = refrenceId
            };

            UnitOfWork.AccountRepository.Insert(account);

           
                account = new Account()
                {
                    BranchId = branchId,
                    Code = code + 1,
                    Bedehkar = totalAmount,
                    Bestankar = 0,
                    Body = "درخواست کالای کد " + orderCode,
                    IsActive = true,
                    Remain = totalAmount,
                    RefrenceId = refrenceId
                };

                UnitOfWork.AccountRepository.Insert(account);
            

        }
    }
}
