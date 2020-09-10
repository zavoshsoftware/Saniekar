using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;
using ViewModels;


namespace Presentation.Controllers
{
    public class ProductRequestController : Infrastructure.BaseControllerWithUnitOfWork
    {
        readonly AmountCalculator _amountCalculator = new AmountCalculator();
        public ActionResult Form()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    List<Branch> branches = new List<Branch>();

                    string role = GetUserRole();

                    if (role == "Administrator")
                    {
                        branches = UnitOfWork.BranchRepository.Get().ToList();
                        ViewBag.BranchId = new SelectList(branches, "Id", "Title");
                    }
                    else
                    {
                        var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

                        User user = UnitOfWork.UserRepository.Get(current => current.CellNum == identity.Name)
                            .FirstOrDefault();

                        if (user != null)
                        {
                            branches = GetUserBranches(user.Id);

                            ViewBag.BranchId = new SelectList(branches, "Id", "Title", branches.FirstOrDefault()?.Id);
                        }
                    }
                }



                ViewBag.BranchId2 = new SelectList(GetSupplierBranches(), "Id", "Title", GetSupplierBranches().FirstOrDefault()?.Id);

                ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(), "Id", "Title");

                InputDocumentViewModel productRequest = new InputDocumentViewModel()
                { InputDate = GetPersianDateTime(DateTime.Now) };

                Deletecookie();
                return View(productRequest);
            }
            catch (Exception e)
            {
                return Redirect("/error.html");
            }
        }
        public DateTime GetPersianDateTime(DateTime date)
        {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            string year = pc.GetYear(date).ToString().PadLeft(4, '0');
            string month = pc.GetMonth(date).ToString().PadLeft(2, '0');
            string day = pc.GetDayOfMonth(date).ToString().PadLeft(2, '0');
            string stringDate = String.Format("{2}/{1}/{0}", year, month, day) + " " + date.ToString("HH:mm:ss");

            //  var value = "1396/11/27";
            // Convert to Miladi
            DateTime dt = DateTime.Parse(stringDate, new System.Globalization.CultureInfo("fa-IR"));
            // Get Utc Date
            return dt.ToUniversalTime();

        }
        public List<Branch> GetSupplierBranches()
        {
            List<User> users = UnitOfWork.UserRepository
                .Get(current => current.Role.Name == "Factory" && current.IsDeleted == false).ToList();


            List<Branch> supplierBranches = new List<Branch>();


            foreach (User user in users)
            {
                BranchUser branchUser = UnitOfWork.BranchUserRepository.Get(current => current.UserId == user.Id)
                    .FirstOrDefault();

                if (branchUser != null)
                {
                    Branch branch = UnitOfWork.BranchRepository.GetById(branchUser.BranchId);

                    supplierBranches.Add(branch);

                    break;
                }
            }

            return supplierBranches;
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

        public string GetUserRole()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string role = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            return role;
        }



        public string[] GetCookie()
        {
            if (Request.Cookies["basket"] != null)
            {
                string cookievalue = Request.Cookies["basket"].Value;

                string[] basketItems = cookievalue.Split('/');

                return basketItems;
            }

            return null;
        }

        public void Deletecookie()
        {
            HttpCookie currentUserCookie = Request.Cookies["basket"];
            Response.Cookies.Remove("basket");
            if (currentUserCookie != null)
            {
                currentUserCookie.Expires = DateTime.Now.AddDays(-10);
                currentUserCookie.Value = null;
                Response.SetCookie(currentUserCookie);
            }
        }



        public void SetCookie(string[] basket)
        {
            string cookievalue = null;

            Deletecookie();

            foreach (string s in basket)
            {
                if (!string.IsNullOrEmpty(s))
                    cookievalue = cookievalue + s + "/";
            }

            HttpContext.Response.Cookies.Set(new HttpCookie("basket")
            {
                Name = "basket",
                Value = cookievalue,
                Expires = DateTime.Now.AddDays(1)
            });
        }


        public DateTime GetGrDate(DateTime datetime)
        {
            System.Globalization.PersianCalendar c = new System.Globalization.PersianCalendar();

            DateTime date = c.ToDateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0, 0);

            return date;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult PostFinalize(string requestBranchId, string requestSupplierId, string orderId,
            string inputDate, string desc)
        {
            try
            {
                DateTime grDate = GetGrDate(Convert.ToDateTime(inputDate));
                // DateTime grDate = (Convert.ToDateTime(inputDate));

                ProductRequest productRequest =
                    InsertProductRequest(requestBranchId, requestSupplierId, orderId, grDate, desc);

                InsertProductRequestDetail(GetProductIdByCookie(productRequest), productRequest);

                UnitOfWork.Save();

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public ProductRequest InsertProductRequest(string branchId, string supplierId, string orderId, DateTime inputDate, string desc)
        {
            CodeGenerator codeGenerator = new CodeGenerator();

            ProductRequest productRequest = new ProductRequest
            {
                RequestBranchId = new Guid(branchId),
                RequestSupplierId = new Guid(supplierId),
                Code = codeGenerator.ReturnProductRequestCode(),
                RequestDate = Convert.ToDateTime(inputDate),
                Description = desc
            };

            UnitOfWork.ProductRequestRepository.Insert(productRequest);

            return productRequest;
        }

        public OrderInsertViewModel GetProductIdByCookie(ProductRequest productRequest)
        {
            string[] coockieProducts = GetCookie();

            List<InputDocumentInsertViewModel> productList = new List<InputDocumentInsertViewModel>();

            decimal subTotal = 0;

            for (int i = 0; i < coockieProducts.Length - 1; i++)
            {
                string[] productFeatures = coockieProducts[i].Split('^');

                Guid id = new Guid(productFeatures[0]);
                //InputDocumentInsertViewModel idiv = productList.FirstOrDefault(current => current.ProductId == id);

                Product oProduct = UnitOfWork.ProductRepository.GetById(id);


                Guid? colorId = null;
                if (productFeatures[2] != "nocolor")
                {
                    colorId = new Guid(productFeatures[2]);
                }

                Guid? mattressId = null;
                if (productFeatures[3] != "nomatterss")
                {
                    mattressId = new Guid(productFeatures[3]);
                }

                int qty = Convert.ToInt32(productFeatures[1]);

                if (oProduct != null)
                {

                    decimal amount = _amountCalculator.GetAmountByType(oProduct, "store");

                    if (colorId != null)
                    {
                        ProductColor productColor = UnitOfWork.ProductColorRepository.GetById(colorId.Value);
                        amount = _amountCalculator.GetAmountByType(oProduct, "store") + productColor.Amount;
                    }

                    decimal rowAmount = qty * amount;

                    subTotal = subTotal + rowAmount;



                    InputDocumentInsertViewModel input = new InputDocumentInsertViewModel()
                    {
                        ProductId = oProduct.Id,
                        Quantity = qty,
                        RowAmount = rowAmount,
                        Amount = amount,
                        ColorId = colorId,
                        MattressId = mattressId
                    };

                    productList.Add(input);
                }
            }

            productRequest.Total = subTotal;

            OrderInsertViewModel orderDetails = new OrderInsertViewModel()
            {
                OrderDetails = productList,
                SubTotal = subTotal
            };

            return orderDetails;
        }

        public void InsertProductRequestDetail(OrderInsertViewModel orderInsert,
            ProductRequest productRequest)
        {
            List<InputDocumentInsertViewModel> products = orderInsert.OrderDetails;

            foreach (InputDocumentInsertViewModel product in products)
            {
                ProductRequestDetail productRequestDetail = new ProductRequestDetail()
                {
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                    RowAmount = product.RowAmount,
                    ProductRequestId = productRequest.Id,
                    Amount = product.Amount,
                    TotalSupplied = 0,
                    ProductColorId = product.ColorId,
                    MattressId = product.MattressId
                };

                UnitOfWork.ProductRequestDetailRepository.Insert(productRequestDetail);
            }
        }


        public ActionResult Index()
        {
            List<Branch> branches = new List<Branch>();

            string role = GetUserRole();

            if (role == "Administrator")
                branches = UnitOfWork.BranchRepository.Get().ToList();

            else
            {
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

                User user = UnitOfWork.UserRepository.Get(current => current.CellNum == identity.Name)
                    .FirstOrDefault();

                if (user != null)
                    branches = GetUserBranches(user.Id);
            }

            List<ProductRequest> productRequests = new List<ProductRequest>();

            foreach (Branch branch in branches)
            {
                List<ProductRequest> oProductRequests = UnitOfWork.ProductRequestRepository
                    .Get(current => current.RequestBranchId == branch.Id).ToList();

                foreach (ProductRequest productRequest in oProductRequests)
                {
                    productRequests.Add(productRequest);
                }
            }
            return View(productRequests);
        }

        public ActionResult Edit(Guid id)
        {
            ProductRequest productRequest = UnitOfWork.ProductRequestRepository.GetById(id);

            List<Branch> branches = new List<Branch>();

            string role = GetUserRole();

            if (role == "Administrator")
            {
                branches = UnitOfWork.BranchRepository.Get().ToList();
                ViewBag.BranchId = new SelectList(branches, "Id", "Title", productRequest.RequestBranchId);
            }
            else
            {
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

                User user = UnitOfWork.UserRepository.Get(current => current.CellNum == identity.Name)
                    .FirstOrDefault();

                if (user != null)
                {
                    branches = GetUserBranches(user.Id);

                    ViewBag.BranchId = new SelectList(branches, "Id", "Title", productRequest.RequestBranchId);
                }
            }

            ViewBag.BranchId2 = new SelectList(GetSupplierBranches(), "Id", "Title", productRequest.RequestSupplierId);

            ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(), "Id", "Title");

            ProductRequestEditViewModel model = new ProductRequestEditViewModel();

            List<ProductRequestDetail> ProductRequestDetails = UnitOfWork.ProductRequestDetailRepository
                .Get(current => current.ProductRequestId == id).ToList();

            Deletecookie();
            List<string> basket = new List<string>();
            foreach (ProductRequestDetail productRequestDetail in ProductRequestDetails)
            {
                for (int i = 0; i < productRequestDetail.Quantity; i++)
                {
                    basket.Add(productRequestDetail.ProductId.ToString());
                }
            }

            SetCookie(basket.ToArray());


            if (productRequest != null)
            {
                model.ProductRequest = productRequest;

                model.ProductRequestDetails = ProductRequestDetails;
            }

            return View(model);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult PostEdit(string requestBranchId, string requestSupplierId, string orderId, string inputDate,
            string desc, string parentId)
        {
            try
            {
                Guid productRequestId = new Guid(parentId);

                ProductRequest productRequest = UnitOfWork.ProductRequestRepository.GetById(productRequestId);

                EditProductRequest(productRequest, requestBranchId, requestSupplierId, orderId, inputDate, desc);

                DeleteProductRequesDetails(productRequest.Id);

                InsertProductRequestDetail(GetProductIdByCookie(productRequest), productRequest);

                UnitOfWork.Save();

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }


        public void EditProductRequest(ProductRequest productRequest, string branchId, string supplierId,
            string orderId, string inputDate, string desc)
        {

            productRequest.RequestBranchId = new Guid(branchId);
            productRequest.RequestSupplierId = new Guid(supplierId);
            productRequest.Code = Convert.ToInt32(orderId);
            productRequest.RequestDate = Convert.ToDateTime(inputDate);
            productRequest.Description = desc;

            UnitOfWork.ProductRequestRepository.Update(productRequest);
        }

        public void DeleteProductRequesDetails(Guid productRequestId)
        {
            List<ProductRequestDetail> productRequestDetails = UnitOfWork.ProductRequestDetailRepository
                .Get(current => current.ProductRequestId == productRequestId).ToList();

            foreach (ProductRequestDetail productRequestDetail in productRequestDetails)
            {
                UnitOfWork.ProductRequestDetailRepository.Delete(productRequestDetail);
            }

        }


        public ActionResult List()
        {
            List<Branch> branches = new List<Branch>();

            string role = GetUserRole();

            if (role == "Administrator")
                branches = UnitOfWork.BranchRepository.Get().ToList();

            else
            {
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

                User user = UnitOfWork.UserRepository.Get(current => current.CellNum == identity.Name)
                    .FirstOrDefault();

                if (user != null)
                    branches = GetUserBranches(user.Id);
            }

            List<ProductRequest> productRequests = new List<ProductRequest>();

            foreach (Branch branch in branches)
            {
                List<ProductRequest> oProductRequests = UnitOfWork.ProductRequestRepository
                    .Get(current => current.RequestSupplierId == branch.Id).ToList();

                foreach (ProductRequest productRequest in oProductRequests)
                {
                    productRequests.Add(productRequest);
                }
            }
            return View(productRequests);
        }


        public ActionResult Detail(Guid id)
        {
            ProductRequest productRequest = UnitOfWork.ProductRequestRepository.GetById(id);

            ProductRequestDetailViewModel productRequestDetailViewModel = new ProductRequestDetailViewModel()
            {
                ProductRequest = productRequest,
                ProductRequestDetails = GetProductRequestDetail(id)
            };

            return View(productRequestDetailViewModel);
        }

        [HttpPost]
        public ActionResult Detail(ProductRequestDetailViewModel productRequestDetailViewModel)
        {
            foreach (ProductRequestDetailItem detail in productRequestDetailViewModel.ProductRequestDetails)
            {
                if (detail.FactoryStock < detail.SupplyNumber)
                {

                    ModelState.AddModelError("moreThanStock",
                        "مقدار وارد شده برای محصول " + detail.ProductTitle +
                        " بیشتر از موجودی انبار کارخانه می باشد.");

                    Guid productRequestId = productRequestDetailViewModel.ProductRequestDetails.FirstOrDefault()
                        .ProductRequestDetailId;

                    ProductRequestDetail productRequestDetail =
                        UnitOfWork.ProductRequestDetailRepository.GetById(productRequestId);
                    ProductRequest productRequest =
                        UnitOfWork.ProductRequestRepository.GetById(productRequestDetail.ProductRequestId);

                    productRequestDetailViewModel.ProductRequest = productRequest;
                    return View(productRequestDetailViewModel);
                }

                if (detail.Remain < detail.SupplyNumber)
                {

                    ModelState.AddModelError("moreThanRemail",
                        "مقدار وارد شده برای محصول " + detail.ProductTitle +
                        " بیشتر از مقدار باقی مانده می باشد.");

                    Guid productRequestId = productRequestDetailViewModel.ProductRequestDetails.FirstOrDefault()
                        .ProductRequestDetailId;

                    ProductRequestDetail productRequestDetail =
                        UnitOfWork.ProductRequestDetailRepository.GetById(productRequestId);
                    ProductRequest productRequest =
                        UnitOfWork.ProductRequestRepository.GetById(productRequestDetail.ProductRequestId);

                    productRequestDetailViewModel.ProductRequest = productRequest;
                    return View(productRequestDetailViewModel);
                }

                if (detail.SupplyNumber > 0)
                {
                    Guid? factoryId = GetFactoryBranchId();

                    Guid productRequestDetailId = productRequestDetailViewModel.ProductRequestDetails.FirstOrDefault()
                        .ProductRequestDetailId;

                    ProductRequestDetail productRequestDetail =
                        UnitOfWork.ProductRequestDetailRepository.GetById(productRequestDetailId);

                    ProductRequest productRequest =
                        UnitOfWork.ProductRequestRepository.GetById(productRequestDetail.ProductRequestId);

                    if (factoryId != null && productRequest != null)
                        InsertOrUpdateSupplier(factoryId.Value, detail.ProductRequestDetailId, detail.SupplyNumber, productRequest.RequestBranchId.Value);


                }
            }
            return RedirectToAction("List");
        }

        public void InsertOrUpdateSupplier(Guid factoryId, Guid productRequestDetailId, int quantity, Guid recieverBranchId)
        {
            //ProductRequestDetailSupplier oProductRequestDetailSupplier = UnitOfWork
            //    .ProductRequestDetailSupplierRepository.Get(current =>
            //        current.BranchId == factoryId && current.ProductRequestDetailId == productRequestDetailId)
            //    .FirstOrDefault();

            ProductRequestDetail productRequestDetail =
                UnitOfWork.ProductRequestDetailRepository.GetById(productRequestDetailId);

            productRequestDetail.TotalSupplied = productRequestDetail.TotalSupplied + quantity;
            UnitOfWork.ProductRequestDetailRepository.Update(productRequestDetail);


            Inventory inventory = UnitOfWork.InventoryRepository.Get(current =>
                    current.BranchId == factoryId && current.ProductId == productRequestDetail.ProductId)
                .FirstOrDefault();

            if (inventory != null)
            {
                inventory.Stock = inventory.Stock - quantity;
                UnitOfWork.InventoryRepository.Update(inventory);
            }
            //if (oProductRequestDetailSupplier == null)
            //{
            ProductRequestDetailSupplier productRequestDetailSupplier = new ProductRequestDetailSupplier()
            {
                BranchId = recieverBranchId,
                ProductRequestDetailId = productRequestDetailId,
                Quantity = quantity,
                IsReceived = false
            };
            UnitOfWork.ProductRequestDetailSupplierRepository.Insert(productRequestDetailSupplier);


            //}
            //else
            //{
            //    int oldQuantity = oProductRequestDetailSupplier.Quantity;
            //    oProductRequestDetailSupplier.Quantity = quantity;
            //    UnitOfWork.ProductRequestDetailSupplierRepository.Update(oProductRequestDetailSupplier);


            //}

            UnitOfWork.Save();

        }

        public Guid? GetFactoryBranchId()
        {
            Guid? factoryId = null;
            User user = UnitOfWork.UserRepository.Get(current => current.Role.Name == "factory").FirstOrDefault();

            if (user != null)
            {
                BranchUser branchUser = UnitOfWork.BranchUserRepository.Get(current => current.UserId == user.Id)
                    .FirstOrDefault();

                if (branchUser != null)
                    factoryId = branchUser.BranchId;

            }

            return factoryId;
        }

        public List<ProductRequestDetailItem> GetProductRequestDetail(Guid productRequestId)
        {
            List<ProductRequestDetail> productRequestDetails = UnitOfWork.ProductRequestDetailRepository
                .Get(current => current.ProductRequestId == productRequestId).ToList();

            List<ProductRequestDetailItem> details = new List<ProductRequestDetailItem>();


            Product product = new Product();

            foreach (ProductRequestDetail requestDetail in productRequestDetails)
            {
                //if (requestDetail.Product.ParentId != null)
                //    product = UnitOfWork.ProductRepository.GetById(requestDetail.Product.ParentId.Value);
                //else
                product = requestDetail.Product;

                details.Add(new ProductRequestDetailItem()
                {
                    ProductRequestDetailId = requestDetail.Id,
                    ProductTitle = product.Title,
                    ProductColorTitle = requestDetail.ProductColor?.Title,
                    MattressTitle = requestDetail.Mattress?.Title,
                    Quantity = requestDetail.Quantity,
                    Amount = requestDetail.AmountStr,
                    RowAmount = requestDetail.RowAmountStr,
                    FactoryStock = GetProductStockByBranch(requestDetail.ProductId, requestDetail.ProductColorId, requestDetail.MattressId, requestDetail.ProductRequest.RequestBranchId.Value, "factory"),
                    BranchStock = GetProductStockByBranch(requestDetail.ProductId, requestDetail.ProductColorId, requestDetail.MattressId, requestDetail.ProductRequest.RequestBranchId.Value, "branch"),
                    //SupplyNumber = GetSupplyNumberByProductReq(requestDetail.Id),
                    Remain = requestDetail.Quantity - requestDetail.TotalSupplied

                });
            }

            return details;
        }

        public int GetSupplyNumberByProductReq(Guid productRequestDetailId)
        {
            Guid? branchId = GetFactoryBranchId();

            ProductRequestDetailSupplier p = UnitOfWork.ProductRequestDetailSupplierRepository
                .Get(current => current.ProductRequestDetailId == productRequestDetailId && current.BranchId == branchId).FirstOrDefault();


            if (p != null)
                return p.Quantity;
            else return 0;
        }

        public int GetProductStockByBranch(Guid productId, Guid? colorId, Guid? mattressId, Guid branchId, string branchType)
        {
            if (branchType.ToLower() == "factory")
            {
                Guid? factoryID = GetFactoryBranchId();
                if (factoryID != null)
                    branchId = factoryID.Value;
                else return 0;
            }

            Inventory inventory = UnitOfWork.InventoryRepository
                .Get(current => current.BranchId == branchId && current.ProductId == productId &&
                                current.ProductColorId == colorId && current.MattressId == mattressId).FirstOrDefault();

            if (inventory != null)
                return inventory.Stock;
            else return 0;

        }
    }
}