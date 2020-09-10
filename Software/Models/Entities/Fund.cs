using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Fund : BaseEntity
    {
        [Display(Name = "شعبه")]
        public Guid BranchId { get; set; }
        public Branch Branch { get; set; }
        [Display(Name = "مبلغ شارژ تنخواه")]
        [Column(TypeName = "Money")]
        public decimal Amount { get; set; }
        [Display(Name = "زمان دریافت")]
        public DateTime ReceiveDate { get; set; }
        [Display(Name = "زمان بستن تنخواه")]
        public DateTime? FinishDate { get; set; }
        [Display(Name = "مبلغ باقی مانده")]
        [Column(TypeName = "Money")]
        public decimal RemainAmount { get; set; }

        [NotMapped]
        [Display(Name = "زمان دریافت")]
        public string ReceiveDateStr
        {
            get
            {
                //  return "hi";
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(ReceiveDate).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(ReceiveDate).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(ReceiveDate).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day);
            }
        }
        [NotMapped]
        [Display(Name = "زمان بستن تنخواه")]
        public string FinishDateStr
        {
            get
            {
                if (FinishDate != null)
                {
                    //  return "hi";
                    System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                    string year = pc.GetYear(FinishDate.Value).ToString().PadLeft(4, '0');
                    string month = pc.GetMonth(FinishDate.Value).ToString().PadLeft(2, '0');
                    string day = pc.GetDayOfMonth(FinishDate.Value).ToString().PadLeft(2, '0');
                    return String.Format("{0}/{1}/{2}", year, month, day);
                }
                return string.Empty;
            }
        }

        [NotMapped]
        [Display(Name = "مبلغ شارژ تنخواه")]
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

        public ICollection<Fund> Funds { get; set; }
    }
}
