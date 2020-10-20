using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Order : BaseEntity
    {
        public Order()
        {
            OrderDetails=new List<OrderDetail>();
        }
      
        [Display(Name = "Code", ResourceType = typeof(Resources.Models.Order))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public int Code { get; set; }

        [Display(Name = "UserId", ResourceType = typeof(Resources.Models.Order))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public Guid UserId { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Models.Order))]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید.")]
        public decimal SubAmount { get; set; }

        [Display(Name = "AdditiveAmount", ResourceType = typeof(Resources.Models.Order))]
        public decimal AdditiveAmount { get; set; }

        [Display(Name = "DiscountAmount", ResourceType = typeof(Resources.Models.Order))]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "TotalAmount", ResourceType = typeof(Resources.Models.Order))]
        public decimal TotalAmount { get; set; }

        [Display(Name = "PaymentAmount", ResourceType = typeof(Resources.Models.Order))]
        public decimal PaymentAmount { get; set; }

        [Display(Name = "RemainAmount", ResourceType = typeof(Resources.Models.Order))]
        public decimal RemainAmount { get; set; }

        [Display(Name = "OrderFile", ResourceType = typeof(Resources.Models.Order))]
        public string OrderFile { get; set; }

        [Display(Name = "CityId", ResourceType = typeof(Resources.Models.Order))]
        public Guid? CityId { get; set; }

        [Display(Name = "Address", ResourceType = typeof(Resources.Models.Order))]
        public string Address { get; set; }

        [Display(Name = "PostalCode", ResourceType = typeof(Resources.Models.Order))]
        public string PostalCode { get; set; }

        [Display(Name = "OrderDate", ResourceType = typeof(Resources.Models.Order))]
        public DateTime OrderDate { get; set; }

        [Display(Name = "RecieveDate", ResourceType = typeof(Resources.Models.Order))]
        public DateTime RecieveDate { get; set; }

        [Display(Name = "IsPaid", ResourceType = typeof(Resources.Models.Order))]
        public bool IsPaid { get; set; }



        [Display(Name = "BranchId", ResourceType = typeof(Resources.Models.Order))]
        public Guid BranchId { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ShipmentType))]
        public Guid? ShipmentTypeId { get; set; }

        public Guid? RegionId { get; set; }
        public Guid OrderStatusId { get; set; }

        [Display(Name = "Phone", ResourceType = typeof(Resources.Models.Order))]
        public string Phone { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }
        public virtual Region Region { get; set; }
        public virtual ShipmentType ShipmentType { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual User User { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }

        [Display(Name="ضمیمه")]
        public string Attachment { get; set; }

        internal class configuration : EntityTypeConfiguration<Order>
        {
            public configuration()
            {
                HasRequired(p => p.User).WithMany(t => t.Orders).HasForeignKey(p => p.UserId);
                HasRequired(p => p.OrderStatus).WithMany(t => t.Orders).HasForeignKey(p => p.OrderStatusId);
                HasOptional(p => p.ShipmentType).WithMany(t => t.Orders).HasForeignKey(p => p.ShipmentTypeId);
                HasOptional(p => p.City).WithMany(t => t.Orders).HasForeignKey(p => p.CityId);
                HasOptional(p => p.Branch).WithMany(t => t.Orders).HasForeignKey(p => p.BranchId);
                HasOptional(p => p.Region).WithMany(t => t.Orders).HasForeignKey(p => p.RegionId);
            }
        }


        [Display(Name = "OrderDate", ResourceType = typeof(Resources.Models.Order))]
        [NotMapped]
        public string OrderDateStr
        {
            get
            {
                //  return "hi";
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(OrderDate).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(OrderDate).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(OrderDate).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day) ;
            }
        }


        [Display(Name = "RecieveDate", ResourceType = typeof(Resources.Models.Order))]
        [NotMapped]
        public string RecieveDateStr
        {
            get
            {
                //  return "hi";
                System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                string year = pc.GetYear(RecieveDate).ToString().PadLeft(4, '0');
                string month = pc.GetMonth(RecieveDate).ToString().PadLeft(2, '0');
                string day = pc.GetDayOfMonth(RecieveDate).ToString().PadLeft(2, '0');
                return String.Format("{0}/{1}/{2}", year, month, day) ;
            }
        }



        [NotMapped]
        [Display(Name = "TotalAmount", ResourceType = typeof(Resources.Models.Order))]
        public string TotalAmountStr
        {
            get { return TotalAmount.ToString("N0"); }
        }

        [NotMapped]
        [Display(Name = "DiscountAmount", ResourceType = typeof(Resources.Models.Order))]
        public string DiscountAmountStr
        {
            get { return DiscountAmount.ToString("N0"); }
        }


        [NotMapped]
        [Display(Name = "AdditiveAmount", ResourceType = typeof(Resources.Models.Order))]
        public string AdditiveAmountStr
        {
            get { return AdditiveAmount.ToString("N0"); }
        }

        [NotMapped]
        public string SubAmountStr
        {
            get { return SubAmount.ToString("N0"); }
        }


        [NotMapped]
        [Display(Name = "PaymentAmount", ResourceType = typeof(Resources.Models.Order))]
        public string PaymentAmountStr
        {
            get { return PaymentAmount.ToString("N0"); }
        }


        [NotMapped]
        [Display(Name = "RemainAmount", ResourceType = typeof(Resources.Models.Order))]
        public string RemainAmountStr
        {
            get { return RemainAmount.ToString("N0"); }
        }

        [Display(Name="ارسال از کارخانه")]
        public bool ShipmentFromFactory { get; set; }

        [Display(Name="توضیحات برای کارخانه")]
        public string FactoryShipmentDesc { get; set; }

        [Display(Name="فاکتور ویرایش شده")]
        public bool IsEdit { get; set; }

        [Display(Name="تغییرات توسط مغازه بررسی شد")]
        public bool CheckByStore { get; set; }

        [Display(Name="تغییرات توسط کارخانه بررسی شد")]
        public bool CheckByFactory { get; set; }
    }
}
