using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reports.Models
{
    public class OrderReportViewModel
    {
        public string Code { get; set; }
        public string Date { get; set; }
        public string TotalAmount { get; set; }
        public string RemainAmount { get; set; }
        public string CustomerFullName { get; set; }
        public string Address { get; set; }
        public string CustomerCellNumber { get; set; }
        public string AdditiveAmount { get; set; }
        public string DecreasedAmount { get; set; }
        public string TotalPayment { get; set; }
        public string Description { get; set; }
        public string DeliverDate { get; set; }
        public string SubTotal { get; set; }
        public IList<OrderDetailReportViewModel> OrderDetails { get; set; }
    }

    public class OrderDetailReportViewModel
    {
        public string Title { get; set; }
        public string Amount { get; set; }
        public string Quantity { get; set; }
        public string RowAmount { get; set; }
        public string ColorTitle { get; set; }
        public string MattressTitle { get; set; }

    }
}