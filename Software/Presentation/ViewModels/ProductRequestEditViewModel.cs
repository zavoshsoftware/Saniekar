using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class ProductRequestEditViewModel
    {
        public ProductRequest ProductRequest { get; set; }
        public List<ProductRequestDetail> ProductRequestDetails { get; set; }
    }

}