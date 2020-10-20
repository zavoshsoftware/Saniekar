using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Presentation.Controllers
{
    [Authorize(Roles = "Branch,Administrator,Factory")]
    public class OrderDetailsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult List()
        {
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            string role = GetUserRole();

            if (role == "Factory")
                orderDetails = UnitOfWork.OrderDetailRepository.Get(c =>
                    c.IsDeleted == false && c.Order.ShipmentFromFactory && c.Order.OrderStatus.Code < 4).ToList();

            if (role == "Branch")
                orderDetails = UnitOfWork.OrderDetailRepository.Get(c =>
                    c.IsDeleted == false && c.Order.ShipmentFromFactory == false && c.Order.OrderStatus.Code < 4).ToList();

            return View(orderDetails);
        }

        public string GetUserRole()
        {
            var identity = (System.Security.Claims.ClaimsIdentity)User.Identity;
            string role = identity.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;

            return role;
        }
    }
}