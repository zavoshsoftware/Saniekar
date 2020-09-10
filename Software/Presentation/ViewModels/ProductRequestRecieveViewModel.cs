using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class ProductRequestRecieveViewModel
    {
        [Display(Name="تاریخ ثبت")]
        public string CreationDateStr { get; set; }

        [Display(Name="تعداد ارسالی")]
        public int Quantity { get; set; }

        [Display(Name="دریافت شده؟")]
        public bool IsRecieved { get; set; }

        [Display(Name="شعبه")]
        public string BranchTitle { get; set; }

        [Display(Name="")]
        public Guid Id { get; set; }

        [Display(Name="محصول")]
        public string ProductTitle { get; set; }
        [Display(Name="رنگ")]
        public string ColorTitle { get; set; }

        [Display(Name="تشک")]
        public string MattressTitle { get; set; }
    }
}