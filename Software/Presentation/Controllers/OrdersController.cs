using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helpers;
using Models;
using ViewModels;
using System.Data.Entity;
using System.Data;
using System.IO;
using System.Net;
using DocumentFormat.OpenXml.Office2010.ExcelAc;

namespace Presentation.Controllers
{
    public class OrdersController : Infrastructure.BaseControllerWithUnitOfWork
    {
        readonly AmountCalculator _amountCalculator = new AmountCalculator();
        public ActionResult Create()
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
                ViewBag.ProvinceId = new SelectList(UnitOfWork.ProvinceRepository.Get(current => current.IsActive).OrderBy(current => current.Title), "Id", "Title");

                ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(), "Id", "Title");

                ViewBag.ShipmentTypeId = new SelectList(UnitOfWork.ShipmentTypeRepository.Get(), "Id", "Title");

                ViewBag.PaymentTypeId = new SelectList(UnitOfWork.PaymentTypeRepository.Get(), "Id", "Title");

                Order order = new Order() { OrderDate = GetPersianDateTime(DateTime.Now), RecieveDate = GetPersianDateTime(DateTime.Now.AddDays(14)) };

                Deletecookie();

                return View(order);
            }
            return RedirectToAction("login", "Account");


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
        [AllowAnonymous]
        [HttpPost]
        public ActionResult PostFinalize(string branchId, string orderDate, string recieveDate, string cellNumber, string fullName, string phone, string address, string cityId, string regionId, string addedAmount,
            string decreasedAmount, string desc, string shipmentTypeId, string paymentAmount, string paymentTypeId, string sendFrom, string factorydesc, string file)
        {
            try
            {
                OrderInsertViewModel orderDetails = GetProductIdByCookie();

                User user = GetCurrentUser(cellNumber, fullName);

                DateTime dtOrderDete = GetGrDate(Convert.ToDateTime(orderDate));
                DateTime dtRecieveDete = GetGrDate(Convert.ToDateTime(recieveDate));

                Order order = InsertOrder(user, branchId, dtOrderDete, dtRecieveDete, phone, address, cityId, regionId,
                    addedAmount, decreasedAmount, desc, shipmentTypeId, paymentAmount, paymentTypeId,
                    orderDetails.SubTotal, sendFrom, factorydesc, file);

                InsertToOrderDetail(orderDetails, order.Id);

                InsertToAccount(order.SubAmount + order.AdditiveAmount - order.DiscountAmount, order.PaymentAmount,
                    order.BranchId, order.UserId, order.Code);

                UnitOfWork.Save();

                return Json("true-" + order.Code, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult UploadFile()
        {
            // check if the user selected a file to upload
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;

                    HttpPostedFileBase file = files[0];

                    string fileName = Path.GetFileName(file.FileName);
                    string newFilename = Guid.NewGuid().ToString().Replace("-", string.Empty)
                                         + Path.GetExtension(fileName);

                    // create the uploads folder if it doesn't exist
                    Directory.CreateDirectory(Server.MapPath("/uploads/attachment/"));
                    string path = Path.Combine(Server.MapPath("/uploads/attachment/"), newFilename);

                    // save the file
                    file.SaveAs(path);
                    return Json("true_/uploads/attachment/" + newFilename);
                }

                catch (Exception e)
                {
                    return Json("error" + e.Message);
                }
            }

            return Json("no files were selected !");
        }

        CodeGenerator codeGenerator = new CodeGenerator();

        public void InsertToAccount(decimal totalAmount, decimal paymentAmount, Guid branchId, Guid userId, int orderCode)
        {
            int code = codeGenerator.ReturnAccountCode();
            Account account = new Account()
            {
                BranchId = branchId,
                Code = code,
                Bedehkar = 0,
                Bestankar = totalAmount,
                Body = "فاکتور فروش کد " + orderCode,
                IsActive = true,
                Remain = totalAmount
            };

            UnitOfWork.AccountRepository.Insert(account);

            if (paymentAmount > 0)
            {
                account = new Account()
                {
                    BranchId = branchId,
                    Code = code + 1,
                    Bedehkar = paymentAmount,
                    Bestankar = 0,
                    Body = "پرداخت اول فاکتور فروش کد " + orderCode,
                    IsActive = true,
                    Remain = totalAmount - paymentAmount
                };

                UnitOfWork.AccountRepository.Insert(account);
            }

        }

        public DateTime GetGrDate(DateTime datetime)
        {
            System.Globalization.PersianCalendar c = new System.Globalization.PersianCalendar();

            DateTime date = c.ToDateTime(datetime.Year, datetime.Month, datetime.Day, 0, 0, 0, 0);

            return date;
        }
        public void InsertToOrderDetail(OrderInsertViewModel orderDetails, Guid orderId)
        {
            foreach (InputDocumentInsertViewModel detail in orderDetails.OrderDetails)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderId = orderId,
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    Amount = detail.Amount,
                    RowAmount = detail.RowAmount,
                    IsActive = true,
                    ProductColorId = detail.ColorId,
                    MattressId = detail.MattressId
                };

                UnitOfWork.OrderDetailRepository.Insert(orderDetail);
            }
        }

        public Order InsertOrder(User user, string branchId, DateTime orderDate, DateTime recieveDate, string phone, string address, string cityId, string regionId, string addedAmount,
            string decreasedAmount, string desc, string shipmentTypeId, string paymentAmount, string paymentTypeId, decimal subTotal, string sendFrom, string factorydesc, string fileUrl)
        {
            decimal additiveAmount = Convert.ToDecimal(addedAmount);
            decimal discountAmount = Convert.ToDecimal(decreasedAmount);
            decimal totalAmount = subTotal + additiveAmount - discountAmount;
            decimal paymentAmountDecimal = Convert.ToDecimal(paymentAmount);
            decimal remainAmountDecimal = totalAmount - paymentAmountDecimal;

            bool isPaid = totalAmount == paymentAmountDecimal;
            Guid? cityIdGuid = null;

            if (!string.IsNullOrEmpty(cityId))
                cityIdGuid = new Guid(cityId);


            Guid branchIdGuid = new Guid(branchId);

            Guid? shippmentTypeIdGuid = null;

            if (!string.IsNullOrEmpty(shipmentTypeId))
                shippmentTypeIdGuid = new Guid(shipmentTypeId);

            bool shipmentFromFactory = sendFrom == "1";

            Order order = new Order()
            {
                Code = ReturnCode(),
                UserId = user.Id,
                SubAmount = subTotal,
                AdditiveAmount = additiveAmount,
                DiscountAmount = discountAmount,
                TotalAmount = totalAmount,
                PaymentAmount = paymentAmountDecimal,
                CityId = cityIdGuid,
                Address = address,
                OrderDate = orderDate,
                RecieveDate = recieveDate,
                IsPaid = isPaid,
                BranchId = branchIdGuid,
                IsActive = true,
                Description = desc,
                ShipmentTypeId = shippmentTypeIdGuid,
                RegionId = GetGuidFromString(regionId),
                RemainAmount = remainAmountDecimal,
                OrderStatusId = UnitOfWork.OrderStatusRepository.Get(current => current.Code == 0).FirstOrDefault().Id,
                Phone = phone,
                ShipmentFromFactory = shipmentFromFactory,
                FactoryShipmentDesc = factorydesc,
                Attachment = fileUrl
            };

            UnitOfWork.OrderRepository.Insert(order);
            UnitOfWork.Save();

            if (paymentAmountDecimal > 0)
                InsertToPayment(paymentAmountDecimal, order.Id, totalAmount, paymentTypeId);

            return order;
        }

        public void InsertToPayment(decimal payAmount, Guid orderId, decimal totalAmount, string paymentTypeId)
        {
            bool isDeposit = payAmount < totalAmount;

            Payment payment = new Payment()
            {
                Amount = payAmount,
                IsDeposit = isDeposit,
                PaymentTypeId = new Guid(paymentTypeId),
                OrderId = orderId,
                IsActive = true

            };

            UnitOfWork.PaymentRepository.Insert(payment);
        }
        public OrderInsertViewModel GetProductIdByCookie()
        {
            string[] coockieProducts = GetCookie();

            List<InputDocumentInsertViewModel> productList = new List<InputDocumentInsertViewModel>();

            decimal subTotal = 0;

            for (int i = 0; i < coockieProducts.Length - 1; i++)
            {
                string[] productFeatures = coockieProducts[i].Split('^');

                Guid id = new Guid(productFeatures[0]);


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
                    decimal amount = _amountCalculator.GetAmountByType(oProduct, "customer");

                    if (colorId != null)
                    {
                        ProductColor productColor = UnitOfWork.ProductColorRepository.GetById(colorId.Value);
                        amount = _amountCalculator.GetAmountByType(oProduct, "customer") + productColor.Amount;
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

            OrderInsertViewModel orderDetails = new OrderInsertViewModel()
            {
                OrderDetails = productList,
                SubTotal = subTotal
            };

            return orderDetails;
        }


        public User GetCurrentUser(string cellNumber, string fullName)
        {
            User user = UnitOfWork.UserRepository
                .Get(current => current.CellNum == cellNumber && current.Role.Name == "customer").FirstOrDefault();

            if (user != null)
            {
                user.FullName = fullName;

                UnitOfWork.UserRepository.Update(user);

                return user;
            }

            user = CreateUser(cellNumber, fullName);

            return user;
        }

        public int RandomCode()
        {
            Random generator = new Random();
            String r = generator.Next(0, 100000).ToString("D5");
            return Convert.ToInt32(r);
        }
        public User CreateUser(string cellNumber, string fullName)
        {
            CodeGenerator codeGenerator = new CodeGenerator();
            User user = new User()
            {
                CellNum = cellNumber,
                FullName = fullName,
                Code = codeGenerator.ReturnOrderCode(),
                IsActive = true,
                Password = RandomCode().ToString(),
                RoleId = UnitOfWork.RoleRepository.Get(current => current.Name == "customer").FirstOrDefault().Id,
            };

            UnitOfWork.UserRepository.Insert(user);

            return user;
        }
        public int ReturnCode()
        {

            Order order = UnitOfWork.OrderRepository.Get().OrderByDescending(current => current.Code).FirstOrDefault();

            if (order != null)
            {
                return order.Code + 1;
            }
            return 1;
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

        public ActionResult GetUserFullName(string cellNumber)
        {
            User user = UnitOfWork.UserRepository.Get(current => current.CellNum == cellNumber && current.Role.Name == "customer").FirstOrDefault();

            if (user == null)
                return Json("invalid", JsonRequestBehavior.AllowGet);

            Order order = UnitOfWork.OrderRepository.Get(c => c.UserId == user.Id)
                .OrderByDescending(c => c.CreationDate).FirstOrDefault();

            string latestAddress = "";
            if (order != null)
                latestAddress = order.Address;

            return Json(user.FullName + "|" + latestAddress, JsonRequestBehavior.AllowGet);
        }


        public ActionResult List(string sent)
        {

            List<Order> orders = new List<Order>();

            if (User.Identity.IsAuthenticated)
            {
                List<Branch> branches = new List<Branch>();

                string role = GetUserRole();

                if (role == "Administrator")
                {
                    branches = UnitOfWork.BranchRepository.Get().ToList();
                    ViewBag.BranchId = new SelectList(branches, "Id", "Title");
                }
                if (role == "Factory")
                {
                    branches = UnitOfWork.BranchRepository.Get().ToList();
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

                foreach (Branch branch in branches)
                {

                    List<Order> brancOrders = UnitOfWork.OrderRepository
                        .Get(current => current.BranchId == branch.Id)
                        .ToList();

                    if (sent == "1")
                    {
                        brancOrders = brancOrders.Where(c => c.OrderStatus.Code == 4).ToList();
                    }
                    else if (sent == "0")
                    {
                        brancOrders = brancOrders.Where(c => c.OrderStatus.Code < 4).ToList();
                    }

                    if (role == "Factory")
                    {
                        brancOrders = brancOrders.Where(c => c.ShipmentFromFactory).ToList();
                    }
                    foreach (Order order in brancOrders)
                    {
                        orders.Add(order);
                    }
                }
            }

            // return orderList.ToList();
            return View(orders
                .OrderByDescending(current => current.OrderStatusId)
                .ThenBy(current => current.CreationDate));
        }

        public ActionResult Cancele(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = UnitOfWork.OrderRepository.GetById(id);

            if (order == null)
            {
                return HttpNotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("Cancele")]
        [ValidateAntiForgeryToken]
        public ActionResult CanceleConfirmed(Guid id)
        {
            Order order = UnitOfWork.OrderRepository.GetById(id);

            order.OrderStatusId = UnitOfWork.OrderStatusRepository.Get(c => c.Code == 5).FirstOrDefault().Id;

            UnitOfWork.OrderRepository.Update(order);

            UnitOfWork.Save();

            return RedirectToAction("List");
        }

        public ActionResult Edit(Guid id)
        {

            Order order = UnitOfWork.OrderRepository.GetById(id);

            if (order == null)
                return HttpNotFound();

            if (User.Identity.IsAuthenticated)
            {
                List<Branch> branches = new List<Branch>();

                string role = GetUserRole();

                if (role == "Administrator")
                {
                    branches = UnitOfWork.BranchRepository.Get().ToList();
                    ViewBag.BranchId = new SelectList(branches, "Id", "Title", order.BranchId);
                }
                else
                {

                    var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

                    User user = UnitOfWork.UserRepository.Get(current => current.CellNum == identity.Name)
                        .FirstOrDefault();

                    if (user != null)
                    {
                        branches = GetUserBranches(user.Id);

                        ViewBag.BranchId = new SelectList(branches, "Id", "Title", order.BranchId);
                    }
                }

                if (order.CityId != null)
                {

                    ViewBag.ProvinceId =
                        new SelectList(
                            UnitOfWork.ProvinceRepository.Get(current => current.IsActive)
                                .OrderBy(current => current.Title), "Id", "Title", order.City?.ProvinceId);

                    ViewBag.CityId =
                        new SelectList(
                            UnitOfWork.CityRepository.Get(current => current.ProvinceId == order.City.ProvinceId)
                                .OrderBy(current => current.Title), "Id", "Title", order.CityId);

                    ViewBag.RegionId =
                        new SelectList(
                            UnitOfWork.RegionRepository.Get(current => current.CityId == order.CityId)
                                .OrderBy(current => current.Title), "Id", "Title", order.RegionId);
                }
                else
                {
                    ViewBag.ProvinceId = new SelectList(UnitOfWork.ProvinceRepository.Get(current => current.IsActive).OrderBy(current => current.Title), "Id", "Title");

                    ViewBag.CityId =
                        new SelectList(
                            UnitOfWork.CityRepository.Get()
                                .OrderBy(current => current.Title), "Id", "Title", order.CityId);

                    ViewBag.RegionId =
                        new SelectList(
                            UnitOfWork.RegionRepository.Get()
                                .OrderBy(current => current.Title), "Id", "Title", order.RegionId);
                }
                ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(), "Id", "Title");

                ViewBag.ShipmentTypeId = new SelectList(UnitOfWork.ShipmentTypeRepository.Get(), "Id", "Title",
                    order.ShipmentTypeId);

                ViewBag.PaymentTypeId = new SelectList(UnitOfWork.PaymentTypeRepository.Get(), "Id", "Title",
                    GetDepositePaymentTypeId(id));

                List<string> basket = new List<string>();

                List<OrderDetail> orderDetails = UnitOfWork.OrderDetailRepository
                    .Get(current => current.OrderId == order.Id).ToList();


                string mattress = "nomatterss";
                string color = "nocolor";

                foreach (OrderDetail orderDetail in orderDetails)
                {
                    if (orderDetail.MattressId != null)
                        mattress = orderDetail.MattressId.ToString();

                    if (orderDetail.ProductColorId != null)
                        color = orderDetail.ProductColorId.ToString();

                    basket.Add(orderDetail.ProductId.ToString() + "^" + orderDetail.Quantity + "^" + color + "^" +
                               mattress);
                }

                SetCookie(basket.ToArray());

                UpdateCheckByRole(role, order);

                OrderEditViewModel orderEdit = new OrderEditViewModel()
                {
                    Order = order,
                    OrderDetails = orderDetails,
                    HasPayment = OrderHasPayment(id)
                };

                return View(orderEdit);
            }

            return RedirectToAction("login", "Account");
        }

        public bool OrderHasPayment(Guid orderId)
        {
            if (UnitOfWork.PaymentRepository.Get(current => current.OrderId == orderId && current.IsDeposit == false)
                .Any())
                return true;

            return false;
        }

        public Guid? GetDepositePaymentTypeId(Guid orderId)
        {
            Payment payment = UnitOfWork.PaymentRepository.Get(current => current.OrderId == orderId && current.IsDeposit == true).FirstOrDefault();

            if (payment != null)
                return payment.PaymentTypeId;
            return null;
        }

        public void UpdateCheckByRole(string role, Order order)
        {
            if (role == "Factory")
            {
                order.CheckByFactory = true;
                UnitOfWork.OrderRepository.Update(order);
                UnitOfWork.Save();
            }
            else if (role == "Branch")
            {
                order.CheckByStore = true;
                UnitOfWork.OrderRepository.Update(order);
                UnitOfWork.Save();
            }

        }

        public ActionResult PostEdit(string branchId, string orderDate, string recieveDate, string cellNumber, string fullName, string phone, string address, string cityId, string regionId, string addedAmount,
            string decreasedAmount, string desc, string shipmentTypeId, string paymentAmount, string paymentTypeId, string parentId, string sendFrom, string factorydesc, string file)
        {
            try
            {
                Guid orderId = new Guid(parentId);

                Order order = UnitOfWork.OrderRepository.GetById(orderId);


                OrderInsertViewModel orderDetails = GetProductIdByCookie();

                EditOrder(order, branchId, orderDate, recieveDate, cellNumber, fullName, phone, address, cityId,
                    regionId, addedAmount, decreasedAmount, desc, shipmentTypeId, paymentAmount, paymentTypeId,
                    orderDetails.SubTotal, sendFrom, factorydesc, file);
                UnitOfWork.Save();

                DeleteOrderDetails(orderId);
                UnitOfWork.Save();

                InsertToOrderDetail(orderDetails, order.Id);

                UnitOfWork.Save();

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CheckInventoryByCode(string code)
        {
            try
            {
                int orderCode = Convert.ToInt32(code);
                Order order = UnitOfWork.OrderRepository.Get(c => c.Code == orderCode).FirstOrDefault();

                bool isInInventory = true;
                if (order != null)
                {
                    List<OrderDetail> orderDetails =
                        UnitOfWork.OrderDetailRepository.Get(c => c.OrderId == order.Id).ToList();

                    var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

                    User user = UnitOfWork.UserRepository.Get(current => current.CellNum == identity.Name)
                        .FirstOrDefault();


                    if (user != null)
                    {
                        Branch branch = GetUserBranches(user.Id).FirstOrDefault();

                        foreach (OrderDetail orderDetail in orderDetails)
                        {
                            Inventory inventory = UnitOfWork.InventoryRepository.Get(c =>
                                c.BranchId == branch.Id && c.ProductId == orderDetail.ProductId &&
                                c.ProductColorId == orderDetail.ProductColorId &&
                                c.MattressId == orderDetail.MattressId).FirstOrDefault();

                            if (inventory == null)
                            {
                                isInInventory = false;
                                break;
                            }
                        }
                    }
                }


                return Json(isInInventory, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }


        public void DeleteOrderDetails(Guid orderId)
        {
            List<OrderDetail> orderDetails = UnitOfWork.OrderDetailRepository
                .Get(current => current.OrderId == orderId).ToList();

            foreach (OrderDetail orderDetail in orderDetails)
            {
                UnitOfWork.OrderDetailRepository.Delete(orderDetail);
            }

        }

        public void EditOrder(Order order, string branchId, string orderDate, string recieveDate, string cellNumber,
            string fullName, string phone, string address, string cityId, string regionId, string addedAmount,
            string decreasedAmount, string desc, string shipmentTypeId, string paymentAmount, string paymentTypeId, decimal subTotal, string sendFrom, string factorydesc, string file)
        {
            User user = GetCurrentUser(cellNumber, fullName);



            decimal additiveAmount = Convert.ToDecimal(addedAmount);
            decimal discountAmount = Convert.ToDecimal(decreasedAmount);
            decimal totalAmount = subTotal + additiveAmount - discountAmount;
            decimal paymentAmountDecimal = Convert.ToDecimal(paymentAmount);
            decimal remainAmountDecimal = totalAmount - paymentAmountDecimal;

            bool isPaid = totalAmount == paymentAmountDecimal;
            order.IsEdit = true;


            order.BranchId = new Guid(branchId);

            if (orderDate != null)
                order.OrderDate = Convert.ToDateTime(orderDate);
            if (recieveDate != null)
                order.RecieveDate = Convert.ToDateTime(recieveDate);

            order.UserId = user.Id;
            order.Phone = phone;
            order.Address = address;
            if (!string.IsNullOrEmpty(cityId))
            {
                order.CityId = new Guid(cityId);
                order.RegionId = GetGuidFromString(regionId);
            }

            bool shipmentFromFactory = sendFrom == "1";

            order.AdditiveAmount = additiveAmount;
            order.DiscountAmount = discountAmount;
            order.Description = desc;

            if (!string.IsNullOrEmpty(shipmentTypeId))
                order.ShipmentTypeId = new Guid(shipmentTypeId);

            order.PaymentAmount = paymentAmountDecimal;
            order.RemainAmount = remainAmountDecimal;
            order.IsPaid = isPaid;
            order.ShipmentFromFactory = shipmentFromFactory;
            order.FactoryShipmentDesc = factorydesc;

            if (!string.IsNullOrEmpty(file))
            {
                order.Attachment = file;
            }

            UnitOfWork.OrderRepository.Update(order);
        }

        public void UpdatePayment(decimal payAmount, Guid orderId, decimal totalAmount, string paymentTypeId)
        {
            Payment payment = UnitOfWork.PaymentRepository
                .Get(current => current.OrderId == orderId && current.IsDeposit).FirstOrDefault();

            if (payment != null)
            {
                payment.Amount = payAmount;
                payment.PaymentTypeId = new Guid(paymentTypeId);

                UnitOfWork.PaymentRepository.Update(payment);
            }
            else
                InsertToPayment(payAmount, orderId, totalAmount, paymentTypeId);
        }

        public Guid? GetGuidFromString(string id)
        {
            Guid? returnId = null;

            if (id != "0" && !string.IsNullOrEmpty(id))
                returnId = new Guid(id);

            return returnId;
        }


        public ActionResult Details(Guid id)
        {
            Order order = UnitOfWork.OrderRepository.GetById(id);

            if (order == null)
                return HttpNotFound();

            if (User.Identity.IsAuthenticated)
            {
                List<Branch> branches = new List<Branch>();

                string role = GetUserRole();

                if (role == "Administrator")
                {
                    branches = UnitOfWork.BranchRepository.Get().ToList();
                    ViewBag.BranchId = new SelectList(branches, "Id", "Title", order.BranchId);
                }

                else
                {
                    var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;

                    User user = UnitOfWork.UserRepository.Get(current => current.CellNum == identity.Name)
                        .FirstOrDefault();

                    if (user != null)
                    {
                        branches = GetUserBranches(user.Id);

                        ViewBag.BranchId = new SelectList(branches, "Id", "Title", order.BranchId);
                    }
                }


                if (order.CityId != null)
                {
                    ViewBag.ProvinceId =
                        new SelectList(
                            UnitOfWork.ProvinceRepository.Get(current => current.IsActive)
                                .OrderBy(current => current.Title), "Id", "Title", order.City.ProvinceId);

                    ViewBag.CityId =
                        new SelectList(
                            UnitOfWork.CityRepository.Get(current => current.ProvinceId == order.City.ProvinceId)
                                .OrderBy(current => current.Title), "Id", "Title", order.CityId);
                }
                else
                {
                    ViewBag.ProvinceId =
                        new SelectList(
                            UnitOfWork.ProvinceRepository.Get(current => current.IsActive)
                                .OrderBy(current => current.Title), "Id", "Title");

                    ViewBag.CityId =
                        new SelectList(
                            UnitOfWork.CityRepository.Get()
                                .OrderBy(current => current.Title), "Id", "Title");

                }
                ViewBag.RegionId =
                    new SelectList(
                        UnitOfWork.RegionRepository.Get(current => current.CityId == order.CityId)
                            .OrderBy(current => current.Title), "Id", "Title", order.RegionId);

                ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(), "Id", "Title");

                ViewBag.ShipmentTypeId = new SelectList(UnitOfWork.ShipmentTypeRepository.Get(), "Id", "Title",
                    order.ShipmentTypeId);

                ViewBag.PaymentTypeId = new SelectList(UnitOfWork.PaymentTypeRepository.Get(), "Id", "Title",
                    GetDepositePaymentTypeId(id));

                List<string> basket = new List<string>();

                List<OrderDetail> orderDetails = UnitOfWork.OrderDetailRepository
                    .Get(current => current.OrderId == order.Id).ToList();

                string mattress = "nomatterss";
                string color = "nocolor";

                foreach (OrderDetail orderDetail in orderDetails)
                {

                    if (orderDetail.MattressId != null)
                        mattress = orderDetail.MattressId.ToString();

                    if (orderDetail.ProductColorId != null)
                        color = orderDetail.ProductColorId.ToString();

                    basket.Add(orderDetail.ProductId.ToString() + "^" + orderDetail.Quantity + "^" + color + "^" +
                               mattress);

                }

                SetCookie(basket.ToArray());
                UpdateCheckByRole(role, order);

                OrderEditViewModel orderEdit = new OrderEditViewModel()
                {
                    Order = order,
                    OrderDetails = orderDetails
                };
                return View(orderEdit);
            }
            return RedirectToAction("login", "Account");
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult SendOrderToFactory(string orderId, string desc)
        {
            try
            {
                Guid id = new Guid(orderId);

                Order order = UnitOfWork.OrderRepository.GetById(id);

                if (order != null)
                {
                    order.ShipmentFromFactory = true;
                    order.FactoryShipmentDesc = desc;
                    order.OrderStatusId = UnitOfWork.OrderStatusRepository.Get(c => c.Code == 2).FirstOrDefault().Id;
                }
                UnitOfWork.OrderRepository.Update(order);
                UnitOfWork.Save();


                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult Sent(Guid id)
        {
            Order order = UnitOfWork.OrderRepository.GetById(id);

            if (order == null)
                return HttpNotFound();

            if (User.Identity.IsAuthenticated)
            {
                string role = GetUserRole();

                if (role.ToLower() == "factory")
                {
                    order.OrderStatusId = UnitOfWork.OrderStatusRepository.Get(c => c.Code == 4).FirstOrDefault().Id;

                    UnitOfWork.OrderRepository.Update(order);
                    UnitOfWork.Save();
                }

                return RedirectToAction("List");
            }
            return RedirectToAction("login", "Account");
        }



        public ActionResult GetAdditiveColorAmount(string parentId, string colorId)
        {
            Guid parentIdGuid = new Guid(parentId);
            Guid colorIdGuid = new Guid(colorId);

            Product product = UnitOfWork.ProductRepository.GetById(parentIdGuid);

            ProductColor productColor = UnitOfWork.ProductColorRepository.Get(c => c.ProductId == parentIdGuid && c.Id == colorIdGuid)
                .FirstOrDefault();

            if (productColor != null)
                return Json((product.Amount + productColor.Amount).Value.ToString("n0"), JsonRequestBehavior.AllowGet);

            return Json("invalid", JsonRequestBehavior.AllowGet);
        }
    }
}