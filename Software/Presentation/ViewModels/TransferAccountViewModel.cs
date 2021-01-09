using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class TransferAccountViewModel
    {
        [Display(Name = "از شعبه")]
        public Guid FromBranchId { get; set; }

        [Display(Name = "به شعبه")]
        public Guid ToBranchId { get; set; }

        [Display(Name = "مبلغ")]
        public decimal Amount { get; set; }

        [Display(Name = "تاریخ انتقال")]
        [UIHint("PersianDatePicker")]
        public DateTime TransferDate { get; set; }

        [Display(Name = "شرح")]
        public string Title { get; set; }
    }

}