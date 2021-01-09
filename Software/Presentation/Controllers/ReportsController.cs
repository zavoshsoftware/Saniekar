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
            Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHn0s4gy0Fr5YoUZ9V00Y0igCSFQzwEqYBh/N77k4f0fWXTHW5rqeBNLkaurJDenJ9o97TyqHs9HfvINK18Uwzsc/bG01Rq+x3H3Rf+g7AY92gvWmp7VA2Uxa30Q97f61siWz2dE5kdBVcCnSFzC6awE74JzDcJMj8OuxplqB1CYcpoPcOjKy1PiATlC3UsBaLEXsok1xxtRMQ283r282tkh8XQitsxtTczAJBxijuJNfziYhci2jResWXK51ygOOEbVAxmpflujkJ8oEVHkOA/CjX6bGx05pNZ6oSIu9H8deF94MyqIwcdeirCe60GbIQByQtLimfxbIZnO35X3fs/94av0ODfELqrQEpLrpU6FNeHttvlMc5UVrT4K+8lPbqR8Hq0PFWmFrbVIYSi7tAVFMMe2D1C59NWyLu3AkrD3No7YhLVh7LV0Tttr/8FrcZ8xirBPcMZCIGrRIesrHxOsZH2V8t/t0GXCnLLAWX+TNvdNXkB8cF2y9ZXf1enI064yE5dwMs2fQ0yOUG/xornE";
            var path = System.Web.HttpContext.Current.Server.MapPath("~/Content/license.key");
            Stimulsoft.Base.StiLicense.LoadFromFile(path);

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