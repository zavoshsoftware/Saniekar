using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Reports.Models;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace Presentation.Controllers
{
    public class ReportsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        // GET: Report
        public ActionResult Index(Guid id)
        {
            TempData["id"] = id;
            return View();
        }

        public ActionResult LoadReportSnapshot()
        {
            Guid orderId = new Guid(TempData["id"].ToString());

            var report = new StiReport();
            report.Load(Server.MapPath("~/Reports/MRT/OrderListReport.mrt"));
            report.RegBusinessObject("OrderObj", GetOrders(orderId));
            //  report.Dictionary.Variables.Add("today", DateTime.Today());
            return StiMvcViewer.GetReportResult(report);

        }
        public virtual ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
        public virtual ActionResult PrintReport()
        {
            return StiMvcViewer.PrintReportResult();
        }

        public virtual ActionResult ExportReport()
        {
            return StiMvcViewer.ExportReportResult();
        }

        public OrderReportViewModel GetOrders(Guid id)
        {
            Order order = UnitOfWork.OrderRepository.GetById(id);

            OrderReportViewModel report = new OrderReportViewModel()
            {
                Code = order.Code.ToString(),
                Description = order.Description,
                RemainAmount = order.RemainAmountStr,
                AdditiveAmount = order.AdditiveAmountStr,
                Address = order.Address,
                CustomerCellNumber = order.User.CellNum,
                CustomerFullName = order.User.FullName,
                Date = order.OrderDateStr,
                DecreasedAmount = order.DiscountAmountStr,
                DeliverDate = order.RecieveDateStr,
                TotalAmount = order.TotalAmountStr,
                TotalPayment = order.PaymentAmountStr,
                OrderDetails = GetOrderDetails(id),
                SubTotal = order.SubAmountStr
            };


            return report;
        }

        public IList<OrderDetailReportViewModel> GetOrderDetails(Guid orderId)
        {
            IList<OrderDetailReportViewModel> orderDetailReport=new List<OrderDetailReportViewModel>();

            List<OrderDetail> orderDetails = UnitOfWork.OrderDetailRepository.Get(c => c.OrderId == orderId).ToList();

            foreach (OrderDetail orderDetail in orderDetails)
            {
                string mattress = "-";
                if (orderDetail.MattressId != null)
                    mattress = orderDetail.Mattress.Title;

                string color = "-";
                if (orderDetail.ProductColorId != null)
                    color = orderDetail.ProductColor.Title;

                orderDetailReport.Add(new OrderDetailReportViewModel()
                {
                    Title = orderDetail.Product.Title,
                    Amount = orderDetail.AmountStr,
                    Quantity = orderDetail.Quantity.ToString(),
                    RowAmount = orderDetail.RowAmountStr,
                    ColorTitle = color,
                    MattressTitle = mattress
                });
            }

            return orderDetailReport;
        }
    }
}