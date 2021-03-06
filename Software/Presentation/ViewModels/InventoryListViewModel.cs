﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class InventoryListViewModel
    {
        [Display(Name="محصول")]
        public string Title { get; set; }
        [Display(Name="رنگ")]
        public string ColorTitle { get; set; }
        [Display(Name="تشک")]
        public string MattressTitle { get; set; }
        [Display(Name="موجودی")]
        public int Quantity { get; set; }
        public bool HasDetail { get; set; }
        public Guid Id { get; set; }
    }
}