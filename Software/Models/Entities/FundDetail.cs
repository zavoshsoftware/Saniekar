using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class FundDetail : BaseEntity
    {
        [Display(Name = "تنخواه")]
        public Guid FundId { get; set; }
        public Fund Fund { get; set; }

        [Display(Name = "بابت")]
        public string Title { get; set; }

        [Display(Name = "مبلغ پرداختی")]
        [Column(TypeName = "Money")]
        public decimal Amount { get; set; }

        [Display(Name = "زمان پرداخت")]
        [UIHint("PersianDatePicker")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "مبلغ باقی مانده تنخواه")]
        [Column(TypeName = "Money")]
        public decimal RemainAmount { get; set; }


        [NotMapped]
        [Display(Name = "مبلغ پرداختی")]
        public string AmountStr
        {
            get { return Amount.ToString("N0") + " تومان"; }
        }
 [NotMapped]
        [Display(Name = "مبلغ باقی مانده تنخواه")]
        public string RemainAmountStr
        {
            get { return RemainAmount.ToString("N0") + " تومان"; }
        }

    }
}
