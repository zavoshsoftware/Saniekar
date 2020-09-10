using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ProductRequestDetailViewModel
    {
        public ProductRequest ProductRequest { get; set; }
        public List<ProductRequestDetailItem> ProductRequestDetails { get; set; }
    }

    public class ProductRequestDetailItem
    {
        public Guid ProductRequestDetailId { get; set; }
        public string ProductTitle { get; set; }
        public string ProductColorTitle { get; set; }
        public string MattressTitle { get; set; }
        public int Quantity { get; set; }
        public string Amount { get; set; }
        public string RowAmount { get; set; }
        public int BranchStock { get; set; }
        public int FactoryStock { get; set; }
        public int SupplyNumber { get; set; }
        public int Remain { get; set; }
    }
}