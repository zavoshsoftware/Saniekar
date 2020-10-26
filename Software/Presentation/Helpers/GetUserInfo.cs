using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL;
using Models;

namespace Helpers
{
    public static class GetUserInfo
    {
        public static string GetUserFullName()
        {
            DatabaseContext db = new DatabaseContext();

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {

                var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.Current.User.Identity;

                Models.User user = db.Users.FirstOrDefault(current => current.CellNum == identity.Name);

                if (user != null)
                    return user.FullName;
            }

            return string.Empty;
        }
        public static int GetBranchUnSentOrders()
        {
            DatabaseContext db = new DatabaseContext();

            var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.Current.User.Identity;

            User user = db.Users.FirstOrDefault(current => current.CellNum == identity.Name);

            if (user != null)
            {

             
                if (user.BranchId != null)
                {
                    int orderCounts = db.Orders.Count(c => c.BranchId == user.BranchId && c.OrderStatus.Code < 4);

                    return orderCounts;
                }
                return 0;

            }

            return -1;
        }

        public static int GetBranchRecieveCount()
        {
            DatabaseContext db = new DatabaseContext();

            var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.Current.User.Identity;

            User user = db.Users.FirstOrDefault(current => current.CellNum == identity.Name);

            if (user != null)
            {
                if (user.BranchId != null)
                {
                    int count = db.ProductRequestDetailSuppliers.Count(c => c.BranchId == user.BranchId && c.IsReceived != true);

                    return count;
                }
                return 0;

            }

            return -1;
        }

        public static int GetEarlyOrdersCount()
        {
            DatabaseContext db = new DatabaseContext();

            var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.Current.User.Identity;

            User user = db.Users.FirstOrDefault(current => current.CellNum == identity.Name);

            if (user != null)
            {

                DateTime earlyDate = DateTime.Today.AddDays(-2);

              
                if (user.BranchId != null)
                {

                    int orderCounts = db.Orders.Count(c =>
                        c.BranchId == user.BranchId && c.OrderStatus.Code < 4 && c.RecieveDate <= earlyDate);

                    return orderCounts;
                }
                return 0;

            }

            return -1;
        }


        public static int GetFactoryOrderCount()
        {
            DatabaseContext db = new DatabaseContext();

            var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.Current.User.Identity;

            User user = db.Users.FirstOrDefault(current => current.CellNum == identity.Name);

            if (user != null)
            {

                 if (user.BranchId != null)
                {
                    Guid sentStatusId = new Guid("18C1B32B-23F5-4F1F-B426-275214535F38");

                    int count = db.Orders.Count(c => c.ShipmentFromFactory && c.OrderStatusId != sentStatusId && c.IsDeleted == false);

                    return count;
                }
                return 0;

            }

            return -1;
        }



        public static int GetFactoryProductRequestCount()
        {
            DatabaseContext db = new DatabaseContext();

            var identity = (System.Security.Claims.ClaimsIdentity)HttpContext.Current.User.Identity;

            User user = db.Users.FirstOrDefault(current => current.CellNum == identity.Name);

            if (user != null)
            {
                if (user.BranchId != null)
                {
                    Guid completeRequestStatusId = new Guid("4E0268B2-8885-44B7-A622-9285B4886192");

                    int count = db.ProductRequests.Count(c =>
                        c.RequestSupplierId == user.BranchId && c.ProductRequestStatusId!=completeRequestStatusId &&
                        c.IsDeleted == false);
                    
                    return count;
                }
                return 0;

            }

            return -1;
        }
    }
}