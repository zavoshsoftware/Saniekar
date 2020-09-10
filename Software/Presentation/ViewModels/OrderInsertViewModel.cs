using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class OrderInsertViewModel
    {
        public List<InputDocumentInsertViewModel> OrderDetails { get; set; }
        public decimal SubTotal { get; set; }
    }
}