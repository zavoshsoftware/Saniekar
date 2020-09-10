using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Helpers;
using Models;
using ViewModels;


namespace Presentation.Controllers
{
    public class InputDocumentController : Infrastructure.BaseControllerWithUnitOfWork
    {
        readonly AmountCalculator _amountCalculator = new AmountCalculator();

        public ActionResult Form()
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
            ViewBag.SupplierId = new SelectList(UnitOfWork.SupplierRepository.Get(), "Id", "Title");

            ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(), "Id", "Title");
            Deletecookie();
            return View();
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
        #region LoadProductByGroupId

        [AllowAnonymous]
        public ActionResult LoadProductByGroupId(string id, string type)
        {
            Guid productGroupId = new Guid(id);

            List<Product> products = UnitOfWork.ProductRepository
                .Get(current =>
                    current.IsActive && current.ProductGroupId == productGroupId/* && current.ParentId == null*/).ToList();

            ProductInputViewModel productInput = GetProductList(products, type);

            return Json(productInput, JsonRequestBehavior.AllowGet);
        }

        public List<ProductItemViewModel> GetProductViewModel(List<Product> products, string type)
        {
            List<ProductItemViewModel> productItems = new List<ProductItemViewModel>();

            foreach (Product product in products)
            {

                productItems.Add(new ProductItemViewModel()
                {
                    Id = product.Id.ToString(),
                    Title = product.Title,
                    Amount = _amountCalculator.GetAmountStrByType(product, type),
                    ImageUrl = WebConfigurationManager.AppSettings["baseUrl"] + product.ImageUrl,
                    AvailableColors = GetChildProducts(UnitOfWork.ProductColorRepository.Get(c => c.ProductId == product.Id).ToList(), "nocolor"),
                    HasMattress = product.HasMattress,
                    MattressItems = GetMattressItems()
                });
            }

            return productItems;
        }



        #endregion


        //[AllowAnonymous]
        //public ActionResult LoadProductChilderen(string id,string type)
        //{
        //    Guid parentId = new Guid(id);

        //    List<Product> products = UnitOfWork.ProductRepository
        //        .Get(current => current.IsActive/* && current.ParentId == parentId*/).ToList();

        //    if (products.Any())
        //    {
        //        ProductInputViewModel productInput = GetProductList(products, parentId, type);

        //        return Json(productInput, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //        return Json("noChild", JsonRequestBehavior.AllowGet);

        //}

        public ProductInputViewModel GetProductList(List<Product> products, string type)
        {

            ProductInputViewModel productInput =
                new ProductInputViewModel()
                {
                    Products = GetProductViewModel(products, type),
                };

            return productInput;
        }

        public BasketViewModel CalculateBasket(string[] coockieProducts, string type)
        {
            List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
            decimal totalAmount = 0;

            foreach (string oProduct in coockieProducts)
            {
                string[] productItems = oProduct.Split('^');

                string productId = productItems[0];


                if (!string.IsNullOrEmpty(productId))
                {
                    string selectedColor = productItems[2];
                    string selectedMattress = productItems[3];

                    Guid id = new Guid(productId);

                    Product product = UnitOfWork.ProductRepository.GetById(id);

                    string[] colorInfo = GetColorInfo(selectedColor);
                    decimal colorAmount = Convert.ToDecimal(colorInfo[1]);

                    decimal amount = _amountCalculator.GetAmountByType(product, type)+ colorAmount;

                    decimal rowAmount = amount * Convert.ToInt32(productItems[1]);

                    totalAmount = Convert.ToDecimal(totalAmount + rowAmount);

                    if (product != null)
                    {

                        //string parentTitle;

                        //if (product.ParentId != null)
                        //    parentTitle = UnitOfWork.ProductRepository.GetById(product.ParentId.Value).Title;
                        //else
                        //    parentTitle = product.Title;
                       



                        basketItems.Add(new BasketItemViewModel()
                        {
                            Id = productId,
                            Amount = (amount).ToString("n0"),
                            ChildTitle = colorInfo[0],
                            // ChildProducts = GetChildProducts(UnitOfWork.ProductColorRepository.Get(c => c.ProductId == product.Id).ToList(), selectedColor),
                            ParentTitle = product.Title,
                            Quantity = productItems[1],
                            RowAmount = rowAmount.ToString("n0"),
                            Description = "",
                            MattressTitle = GetMattressTitle(selectedMattress)
                        });
                    }
                }
            }

            BasketViewModel basket = GetBasket(basketItems, totalAmount);

            return basket;
        }


        public string[] GetColorInfo(string idString)
        {
            string[] colorInfo;
            if (idString != "nocolor")
            {
                Guid id = new Guid(idString);

                ProductColor productColor = UnitOfWork.ProductColorRepository.GetById(id);

                if (productColor != null)
                    colorInfo = new[] { productColor.Title, productColor.Amount.ToString() };
                else
                    colorInfo = new[] { "", "0" };
            }
            else
                colorInfo = new[] { "", "0" };

            return colorInfo;
        }
        public string GetMattressTitle(string idString)
        {
            if (idString != "nomatterss")
            {
                Guid id = new Guid(idString);

                Mattress mattress = UnitOfWork.MattressRepository.GetById(id);

                return mattress.Title;
            }
            return string.Empty;
        }
        public List<ColorKeyValue> GetChildProducts(List<ProductColor> childs, string selectedId)
        {

            List<ColorKeyValue> childProducts = new List<ColorKeyValue>();
            bool selected = false;
            foreach (ProductColor productColor in childs)
            {
                if (selectedId != "nocolor")
                {
                    Guid selectIdGuid = new Guid(selectedId);
                    if (productColor.Id == selectIdGuid)
                        selected = true;
                }

                childProducts.Add(new ColorKeyValue()
                {
                    Color = productColor.Title,
                    Value = productColor.Id.ToString(),
                    IsSelected = selected
                });
            }

            return childProducts;
        }
        public List<ColorKeyValue> GetMattressItems()
        {
            List<ColorKeyValue> childProducts = new List<ColorKeyValue>();

            List<Mattress> mattresses = UnitOfWork.MattressRepository.Get(c => c.IsActive).ToList();

            foreach (Mattress mattress in mattresses)
            {
                childProducts.Add(new ColorKeyValue()
                {
                    Color = mattress.Title,
                    Value = mattress.Id.ToString()
                });
            }

            return childProducts;
        }

        [AllowAnonymous]
        public ActionResult GetBasketInfoByCookie(string type)
        {
            string[] coockieProducts = GetCookie();
            BasketViewModel basket = CalculateBasket(coockieProducts, type);
            return Json(basket, JsonRequestBehavior.AllowGet);


        }

        public BasketViewModel GetBasket(List<BasketItemViewModel> basketITems, decimal totalAmount)
        {
            BasketViewModel basket = new BasketViewModel()
            {
                Products = basketITems,
                Total = totalAmount.ToString("n0") + " تومان"
            };

            return basket;

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


        [AllowAnonymous]
        public ActionResult RemoveFromBasket(string id, string type)
        {
            string[] coockieProducts = GetCookie();

            foreach (string productId in coockieProducts)
            {
                if (!string.IsNullOrEmpty(productId))
                {
                    if (productId.Split('^')[0] == id)
                    {
                        coockieProducts = coockieProducts.Where(current => current != productId).ToArray();
                        break;
                    }
                }
            }

            SetCookie(coockieProducts);

            BasketViewModel basket = CalculateBasket(coockieProducts, type);

            return Json(basket, JsonRequestBehavior.AllowGet);
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



        [AllowAnonymous]
        public ActionResult UpdateFactor(string rowData, string qtyData)
        {
            string[] row = rowData.Split('/');

            decimal sum = 0;

            for (int i = 0; i < row.Length - 1; i++)
                sum = sum + Convert.ToDecimal(row[i]);

            UpdateCookie(qtyData);
            return Json(sum.ToString("n0") + " تومان", JsonRequestBehavior.AllowGet);
        }

        public void UpdateCookie(string qtyData)
        {
            string[] row = qtyData.Split('/');

            List<string> basket = new List<string>();
            for (int i = 0; i < row.Length - 1; i++)
            {
                string[] rowItem = row[i].Split(',');

                string id = rowItem[0];

                int qty = Convert.ToInt32(rowItem[1]);

                for (int j = 0; j < qty; j++)
                {
                    basket.Add(id);
                }
            }

            SetCookie(basket.ToArray());

        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult PostFinalize(string branchId, string supplierId, string orderId, string inputDate, string addedAmount,
            string decreasedAmount, string desc)
        {
            try
            {
                DateTime grDate = GetGrDate(Convert.ToDateTime(inputDate));

                InputDocument inputDocument = InsertInputDoc(branchId, supplierId, orderId, grDate, addedAmount, decreasedAmount, desc);

                InsertDocumentDetail(GetProductIdByCookie(inputDocument, "factory"), inputDocument);

                UnitOfWork.Save();
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {


                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public DateTime GetGrDate(DateTime datetime)
        {
            System.Globalization.PersianCalendar c = new System.Globalization.PersianCalendar();

            DateTime date = c.ToDateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0, 0);

            return date;
        }
        public InputDocument InsertInputDoc(string branchId, string supplierId, string orderId, DateTime inputDate, string addedAmount,
            string decreasedAmount, string desc)
        {
            InputDocument inputDocument = new InputDocument
            {
                BranchId = new Guid(branchId),
                SupplierId = new Guid(supplierId),
                Code = orderId,
                InputDate = (inputDate),
                AddedAmount = Convert.ToDecimal(addedAmount),
                DecreaseAmount = Convert.ToDecimal(decreasedAmount),
                Description = desc
            };
            UnitOfWork.InputDocumentRepository.Insert(inputDocument);

            return inputDocument;
        }

        public OrderInsertViewModel GetProductIdByCookie(InputDocument inputDocument, string type)
        {
            string[] coockieProducts = GetCookie();

            List<InputDocumentInsertViewModel> productList = new List<InputDocumentInsertViewModel>();

            decimal subTotal = 0;

            for (int i = 0; i < coockieProducts.Length - 1; i++)
            {
                string[] productFeatures = coockieProducts[i].Split('^');

                Guid id = new Guid(productFeatures[0]);

             //   InputDocumentInsertViewModel idiv = productList.FirstOrDefault(current => current.ProductId == id);

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
                    decimal amount = _amountCalculator.GetAmountByType(oProduct, type);

                   


                    if (colorId != null)
                    {
                        ProductColor productColor = UnitOfWork.ProductColorRepository.GetById(colorId.Value);
                        amount = _amountCalculator.GetAmountByType(oProduct, type) + productColor.Amount;
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

            inputDocument.SubTotal = subTotal;
            inputDocument.Total = subTotal + inputDocument.AddedAmount - inputDocument.DecreaseAmount;
            // UnitOfWork.InputDocumentRepository.Update(inputDocument);


            OrderInsertViewModel orderDetails = new OrderInsertViewModel()
            {
                OrderDetails = productList,
                SubTotal = subTotal
            };

            return orderDetails;
        }

        public void InsertDocumentDetail(OrderInsertViewModel orderInsert, InputDocument inputDocument)
        {
            List<InputDocumentInsertViewModel> products = orderInsert.OrderDetails;
            foreach (InputDocumentInsertViewModel product in products)
            {
                InputDocumentDetail inputDocumentDetail = new InputDocumentDetail()
                {
                    ProductId = product.ProductId,
                    Quantity = product.Quantity,
                    RowAmount = product.RowAmount,
                    InputDocumentId = inputDocument.Id,
                    Amount = product.Amount,
                    ProductColorId = product.ColorId,
                    MattressId = product.MattressId
                };
 

                UnitOfWork.InputDocumentDetailRepository.Insert(inputDocumentDetail);
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

            List<InputDocument> inputDocuments = new List<InputDocument>();

            foreach (Branch branch in branches)
            {
                List<InputDocument> oInputDocument = UnitOfWork.InputDocumentRepository
                    .Get(current => current.BranchId == branch.Id).ToList();

                foreach (InputDocument inputDocument in oInputDocument)
                {
                    inputDocuments.Add(inputDocument);
                }
            }
            return View(inputDocuments);
        }

        public ActionResult InputDocumentEdit(Guid id)
        {
            InputDocument inputDocument = UnitOfWork.InputDocumentRepository.GetById(id);

            List<Branch> branches = new List<Branch>();

            string role = GetUserRole();

            if (role == "Administrator")
            {
                branches = UnitOfWork.BranchRepository.Get().ToList();
                ViewBag.BranchId = new SelectList(branches, "Id", "Title", inputDocument.BranchId);
            }
            else
            {
                var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

                User user = UnitOfWork.UserRepository.Get(current => current.CellNum == identity.Name)
                    .FirstOrDefault();

                if (user != null)
                {
                    branches = GetUserBranches(user.Id);

                    ViewBag.BranchId = new SelectList(branches, "Id", "Title", inputDocument.BranchId);
                }
            }

            ViewBag.SupplierId = new SelectList(UnitOfWork.SupplierRepository.Get(), "Id", "Title", inputDocument.SupplierId);

            ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(), "Id", "Title");



            InputDocumentEditViewModel model = new InputDocumentEditViewModel();

            List<InputDocumentDetail> inputDocumentDetails = UnitOfWork.InputDocumentDetailRepository
                .Get(current => current.InputDocumentId == id).ToList();

            Deletecookie();
            List<string> basket = new List<string>();
            foreach (InputDocumentDetail inputDocumentDetail in inputDocumentDetails)
            {
                for (int i = 0; i < inputDocumentDetail.Quantity; i++)
                {
                    basket.Add(inputDocumentDetail.ProductId.ToString());
                }
            }

            SetCookie(basket.ToArray());


            if (inputDocument != null)
            {
                model.InputDocument = inputDocument;

                model.InputDocumentDetails = inputDocumentDetails;
            }

            return View(model);
        }



        [AllowAnonymous]
        [HttpPost]
        public ActionResult PostEdit(string branchId, string supplierId, string orderId, string inputDate, string addedAmount,
            string decreasedAmount, string desc, string parentId)
        {
            try
            {
                Guid inputDocumentId = new Guid(parentId);

                InputDocument inputDocument = UnitOfWork.InputDocumentRepository.GetById(inputDocumentId);

                EditInputDoc(inputDocument, branchId, supplierId, orderId, inputDate, addedAmount, decreasedAmount, desc);

                DeleteInputDocumentDetails(inputDocument.Id);

                InsertDocumentDetail(GetProductIdByCookie(inputDocument, "factory"), inputDocument);

                UnitOfWork.Save();

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }


        public void EditInputDoc(InputDocument inputDocument, string branchId, string supplierId, string orderId, string inputDate, string addedAmount,
            string decreasedAmount, string desc)
        {

            inputDocument.BranchId = new Guid(branchId);
            inputDocument.SupplierId = new Guid(supplierId);
            inputDocument.Code = orderId;
            inputDocument.InputDate = Convert.ToDateTime(inputDate);
            inputDocument.AddedAmount = Convert.ToDecimal(addedAmount);
            inputDocument.DecreaseAmount = Convert.ToDecimal(decreasedAmount);
            inputDocument.Description = desc;

            UnitOfWork.InputDocumentRepository.Update(inputDocument);
        }

        public void DeleteInputDocumentDetails(Guid inputDocumentId)
        {
            List<InputDocumentDetail> inputDocumentDetails = UnitOfWork.InputDocumentDetailRepository
                .Get(current => current.InputDocumentId == inputDocumentId).ToList();

            foreach (InputDocumentDetail inputDocumentDetail in inputDocumentDetails)
            {
                UnitOfWork.InputDocumentDetailRepository.Delete(inputDocumentDetail);
            }

        }
    }
}