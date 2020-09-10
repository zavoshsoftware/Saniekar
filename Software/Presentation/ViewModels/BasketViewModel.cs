using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class BasketViewModel
    {
        public List<BasketItemViewModel> Products { get; set; }
        public string Total { get; set; }
    }

    public class BasketItemViewModel
    {
        public string Id { get; set; }
        public string ParentTitle { get; set; }
        public string ChildTitle { get; set; }
       // public List<ColorKeyValue> ChildProducts { get; set; }
        public string Amount { get; set; }
        public string Quantity { get; set; }
        public string RowAmount { get; set; }
        public string Description { get; set; }
        public string MattressTitle { get; set; }

        //public bool HasMattress { get; set; }
        //public List<ColorKeyValue> MattressItems { get; set; }

    }

    public class ColorKeyValue
    {
        public string Color { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
    }
}