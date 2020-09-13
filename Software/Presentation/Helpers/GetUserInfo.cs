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
            
                BranchUser  branchUser =db.BranchUsers.FirstOrDefault(current => current.UserId == user.Id&&current.IsDeleted==false);

                if (branchUser != null)
                {
                    int orderCounts = db.Orders.Count(c => c.BranchId == branchUser.BranchId && c.OrderStatus.Code < 4);

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
            
                BranchUser  branchUser =db.BranchUsers.FirstOrDefault(current => current.UserId == user.Id&&current.IsDeleted==false);

                if (branchUser != null)
                {
                    int count = db.ProductRequestDetailSuppliers.Count(c => c.BranchId == branchUser.BranchId && c.IsReceived!=true);

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

                BranchUser branchUser = db.BranchUsers.FirstOrDefault(current => current.UserId == user.Id && current.IsDeleted == false);

                if (branchUser != null)
                {

                    int orderCounts = db.Orders.Count(c =>
                        c.BranchId == branchUser.BranchId && c.OrderStatus.Code < 4 && c.RecieveDate <= earlyDate);

                    return orderCounts;
                }
                return 0;

            }

            return -1;
        }
    }
}